using CarGame.WaypointLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CarGame
{
    public class WaypointManagerWindow : EditorWindow
    {
        [MenuItem("Tools/Waypoint Editor")]
        public static void Open()
        {
            GetWindow<WaypointManagerWindow>();
        }

        public Transform waypointRoot; // parent for our waypoints

        private void OnGUI()
        {
            SerializedObject obj = new SerializedObject(this);

            EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

            if (waypointRoot == null)
            {
                EditorGUILayout.HelpBox("Root transform must be selected. Assign a ROOT TRANSFORM", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.BeginVertical("box");
                DrawButtons();
                EditorGUILayout.EndVertical();
            }

            obj.ApplyModifiedProperties();
        }

        void DrawButtons()
        {
            // Create a button and a method for adding a waypoint
            if(GUILayout.Button("Create Waypoint"))
            {
                CreateWayPoint();
            }
        }

        void CreateWayPoint()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));

            // set this way point as a child of our waypoint root
            waypointRoot.transform.SetParent(waypointRoot, false);

            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

            // We want our child waypoints to link to each other
            if(waypointRoot.childCount > 1)
            {
                waypoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
                waypoint.previousWaypoint.nextWaypoint = waypoint;
                // Place the waypoint at the last position
                waypoint.transform.position = waypoint.previousWaypoint.transform.position;
                // Set the waypoints to face in same direction and previous waypoint
                waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
            }

            Selection.activeGameObject = waypoint.gameObject;
        }
    }
}
