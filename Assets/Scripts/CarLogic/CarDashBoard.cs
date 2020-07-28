using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarGame.CarLogic
{
    public class CarDashBoard : MonoBehaviour
    {
        [SerializeField] float maxMotorTorque = 80f;
        [SerializeField] float currentMotorTorque = 0f;

        void Start()
        {
            currentMotorTorque = maxMotorTorque;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public float GetCurrentMotorTorque(float trackTorque)
        {
            trackTorque = currentMotorTorque;
            return trackTorque;
        }

        public float GetCurrentMaxMotorTorque(float trackMaxTorque)
        {
            trackMaxTorque = maxMotorTorque;
            return trackMaxTorque;
        }

        public float SetCurrentMaxMotorTorque(float trackMaxTorque)
        {
            maxMotorTorque = trackMaxTorque;
            return maxMotorTorque;
        }

        public float SetCurrentTorque(float trackTorque)
        {
            currentMotorTorque = trackTorque;
            return currentMotorTorque;
        }
    }
}
