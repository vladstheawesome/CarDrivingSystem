using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarGame.CarLogic
{
    public class CarBrain : MonoBehaviour
    {
        [SerializeField] ObstacleType obstacleType;
        [SerializeField] SpeedLimit speedLimit;

        private float trackTorque;

        CarDashBoard controlCar;

        private void Start()
        {
            controlCar = GetComponent<CarDashBoard>();
        }

        public object CheckObstacle(Collider collider)
        {
            //throw new NotImplementedException();

            if (ObstacleType.Vehicle.ToString() == collider.gameObject.tag.ToString())
            {
                // first get the GameObject of the CURRENT car
                var thisCar = this.transform.GetComponent<CarEngine>();
                var frontCar = collider.gameObject.transform.parent;
                thisCar.isAvoiding = false;

                // check distance from thisCar to FRONT car
                var distanceBetweenCars = thisCar.transform.position.z - frontCar.transform.position.z;

                if (distanceBetweenCars < 8f)
                { 
                    float colliderTorque = frontCar.GetComponent<CarDashBoard>().GetCurrentMotorTorque(trackTorque);
                    controlCar.SetCurrentTorque(colliderTorque);
                    thisCar.ApplyDrive(colliderTorque);
                }
            }

            if (ObstacleType.TrafficLight.ToString() == collider.gameObject.tag.ToString())
            {
                // Traffic light rules
            }

            #region
            if (ObstacleType.RoadObject.ToString() == collider.gameObject.tag.ToString())
            {
                var thisCar = this.transform.GetComponent<CarEngine>();
                thisCar.isAvoiding = true;

                return collider;
            }
            #endregion

            return collider;
        }
    }
}
