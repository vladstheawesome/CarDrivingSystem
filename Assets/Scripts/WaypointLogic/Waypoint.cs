using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarGame.WaypointLogic
{
    public class Waypoint : MonoBehaviour
    {
        public Waypoint previousWaypoint;
        public Waypoint nextWaypoint;

        [Range(0f, 5f)]
        public float width = 1f;

        public Vector3 GetPosition()
        {

        }
    }
}
