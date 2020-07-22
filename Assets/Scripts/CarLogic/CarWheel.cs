using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarGame.CarLogic
{
    public class CarWheel : MonoBehaviour
    {
        public WheelCollider targetWheel;

        private Vector3 wheelPosition = new Vector3();
        private Quaternion wheelRotation = new Quaternion();

        void Update()
        {
            // Get the position and rotation of the wheel colliders and 
            // set them on to the wheels of the car

            targetWheel.GetWorldPose(out wheelPosition, out wheelRotation);
            transform.position = wheelPosition;
            transform.rotation = wheelRotation;
        }
    }
}
