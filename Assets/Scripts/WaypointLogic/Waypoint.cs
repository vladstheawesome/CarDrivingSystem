using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarGame.WaypointLogic
{
    public class Waypoint : MonoBehaviour
    {
        public Waypoint previousWaypoint;
        public Waypoint nextWaypoint;

        [Range(0f, 20f)]
        public float width = 1f;

        public List<Waypoint> branches = new List<Waypoint>();

        [Range(0f, 1f)]
        public float branchRatio = 0.5f;

        public Vector3 GetPosition()
        {
            // Return a random point based on the waypoint width
            // To give some degree of freedom for the pedestrains to move between
            // when they are moving towards a waypoint

            Vector3 minBound = transform.position + transform.right * width / 2f;
            Vector3 maxBound = transform.position - transform.right * width / 2f;

            return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
        }
    }
}
