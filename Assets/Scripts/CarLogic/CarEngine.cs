using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR;

namespace CarGame.CarLogic
{
    public class CarEngine : MonoBehaviour
    {
        public Transform path;
        public float maxSteerAngle = 95f;
        public float turnSpeed = 5f;
        public WheelCollider wheelFL;
        public WheelCollider wheelFR;
        public WheelCollider wheelRL;
        public WheelCollider wheelRR;
        public float maxMotorTorque = 80f;
        public float maxBrakeTorque = 150f;
        public float currentSpeed;
        public float maximumSpeed = 100f;
        public Vector3 centerOfMass;
        public bool isBraking = false;
        public Texture2D textureNormal;
        public Texture2D textureBraking;
        public Renderer carRenderer;

        [Header("Sensors")]
        public float sensorLength = 5f;
        public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
        public float frontSideSensorPosition = 0.85f;
        public float frontSensorAngle = 10;

        private List<Transform> nodes;
        private int currentNode = 0;
        private bool isAvoiding = false;
        private float targetSteerAngle = 0;

        void Start()
        {
            GetComponent<Rigidbody>().centerOfMass = centerOfMass;

            Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
            nodes = new List<Transform>();
            EngineGetNodes(pathTransforms, nodes);
        }

        // Get all nodes from current path
        private void EngineGetNodes(Transform[] pathTransforms, List<Transform> nodes)
        {
            for (int i = 0; i < pathTransforms.Length; i++)
            {
                if (pathTransforms[i] != path.transform)
                {
                    nodes.Add(pathTransforms[i]);
                }
            }
        }

        private void FixedUpdate()
        {
            ObstacleChecker();
            ApplySteer();
            ApplyDrive();
            CheckWaypointDistance();
            ApplyBraking();
            LerpToSteerAngle();
        }

        private void ObstacleChecker()
        {
            RaycastHit hit;
            Vector3 sensorStartPosition = transform.position;
            sensorStartPosition += transform.forward * frontSensorPosition.z;
            sensorStartPosition += transform.up * frontSensorPosition.y;

            // if car senses an object on left, this value is negative
            // if car senses an object on right, this value is positive
            float avoidMultiplier = 0;
            isAvoiding = false;

            if (avoidMultiplier == 0)
            {
                if (Physics.Raycast(sensorStartPosition, transform.forward, out hit, sensorLength))
                {
                    if (!hit.collider.CompareTag("Terrain"))
                    {
                        Debug.DrawLine(sensorStartPosition, hit.point);
                        isAvoiding = true;
                    }
                }
            }

            // Front Right sensor
            sensorStartPosition += transform.right * frontSideSensorPosition;
            if (Physics.Raycast(sensorStartPosition, transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPosition, hit.point);
                    isAvoiding = true;
                    avoidMultiplier -= 1f;
                }
            }

            // Front Right Angle sensor
            else if (Physics.Raycast(sensorStartPosition, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPosition, hit.point);
                    isAvoiding = true;
                    avoidMultiplier -= 0.5f;
                }
            }

            // Front Left sensor
            sensorStartPosition -= transform.right * frontSideSensorPosition * 2;
            if (Physics.Raycast(sensorStartPosition, transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPosition, hit.point);
                    isAvoiding = true;
                    avoidMultiplier += 1f;
                }
            }

            // Front Left Angle sensor
            else if (Physics.Raycast(sensorStartPosition, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPosition, hit.point);
                    isAvoiding = true;
                    avoidMultiplier += 0.5f;
                }
            }

            // Front sensor
            // An obstacle dead center of the car, we still need to determine which way to turn
            //if (avoidMultiplier == 0)
            //{
            //    if (Physics.Raycast(sensorStartPosition, transform.forward, out hit, sensorLength))
            //    {
            //        if (!hit.collider.CompareTag("Terrain"))
            //        {
            //            Debug.DrawLine(sensorStartPosition, hit.point);
            //            isAvoiding = true;
            //        }
            //    }
            //}

            if (isAvoiding)
            {
                targetSteerAngle = maxSteerAngle * avoidMultiplier;
                //wheelFL.steerAngle = maxSteerAngle * avoidMultiplier;
                //wheelFR.steerAngle = maxSteerAngle * avoidMultiplier;
            }
        }

        private void ApplySteer()
        {
            if (isAvoiding) return;

            // Position or Direction of the current node car is driving towards
            Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);

            // TO determine if we are steering LEFT or RIGHT (relativeVector.x / relativeVector.magnitude)
            // - negative (we are steering left)
            // + positive (we are steeting right)

            float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;

            // Set the steer direction onto the Wheel Colliders
            // to make sure they are turning in the right direction
            targetSteerAngle = newSteer;
        }

        private void ApplyDrive()
        {
            // Calculate car speed based on how fast the wheels are spinning
            currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

            if (currentSpeed < maximumSpeed && !isBraking)
            {
                wheelFL.motorTorque = maxMotorTorque;
                wheelFR.motorTorque = maxMotorTorque;
            }
            else
            {
                wheelFL.motorTorque = 0;
                wheelFR.motorTorque = 0;
            }
        }

        private void CheckWaypointDistance()
        {
            var distanceToNode = Vector3.Distance(transform.position, nodes[currentNode].position);

            if (distanceToNode < 12f && distanceToNode > 3f)
            {
                isBraking = true;
            }

            // Drive to the NEXT Node if car has arrived on CURRENT node 
            // (or close to current node by 0.2f)
            if (Vector3.Distance(transform.position, nodes[currentNode].position) < 3f)
            {
                isBraking = false;
                // if we at the LAST node of the path
                // we need to loop back to the START or FIRST node
                if (currentNode == nodes.Count - 1)
                {                    
                    currentNode = 0;
                }
                // Move to NEXT node on path
                else
                {
                    currentNode++;
                    isBraking = false;
                }
            }
        }

        private void ApplyBraking()
        {
            if (isBraking)
            {
                //carRenderer.material.mainTexture = textureBraking;
                // TODO: disable rearlight gameobjects
                // and enable realight BRAKING gameobjects
                wheelRL.brakeTorque = maxBrakeTorque;
                wheelRR.brakeTorque = maxBrakeTorque;
            }
            else
            {
                //carRenderer.material.mainTexture = textureNormal;
                // TODO: vice versa above
                wheelRL.brakeTorque = 0;
                wheelRR.brakeTorque = 0;
            }
        }

        private void LerpToSteerAngle()
        {
            wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
            wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        }
    }
}
