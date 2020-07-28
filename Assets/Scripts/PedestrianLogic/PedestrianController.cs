using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarGame.PedestrianLogic
{
    public class PedestrianController : MonoBehaviour
    {
        [SerializeField] public float movementSpeed;
        [SerializeField] public float rotationSpeed;
        [SerializeField] public float stopDistance;
        [SerializeField] public Vector3 destination;
        public bool reachedDestination;

        Vector3 velocity;
        Vector3 lastPosition;

        // Start is called before the first frame update
        void Start()
        {
            lastPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            // TEMP

            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopDistance)
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
            else
            {
                reachedDestination = true;
            }

            velocity = (transform.position - lastPosition) / Time.deltaTime;
            velocity.y = 0;
            var velocityMagnitude = velocity.magnitude;
            velocity = velocity.normalized;
            var fwdDot = Vector3.Dot(transform.forward, velocity);
            var rightDot = Vector3.Dot(transform.right, velocity);

            // TODO: animate walking
        }

        public void SetDestination(Vector3 destination)
        {
            this.destination = destination;
            reachedDestination = false;
        }
    }
}
