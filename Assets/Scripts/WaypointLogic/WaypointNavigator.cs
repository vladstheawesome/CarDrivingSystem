using CarGame.PedestrianLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarGame.WaypointLogic
{
    public class WaypointNavigator : MonoBehaviour
    {
        PedestrianController controller;
        public Waypoint currentWaypoint;

        private void Awake()
        {
            controller = GetComponent<PedestrianController>();
        }

        // Start is called before the first frame update
        void Start()
        {
            controller.SetDestination(currentWaypoint.GetPosition());
        }

        // Update is called once per frame
        void Update()
        {
            if (controller.reachedDestination)
            {
                currentWaypoint = currentWaypoint.nextWaypoint;
                controller.SetDestination(currentWaypoint.GetPosition());
            }
        }
    }
}
