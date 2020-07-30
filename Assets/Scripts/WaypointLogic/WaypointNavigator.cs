﻿using CarGame.PedestrianLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarGame.WaypointLogic
{
    public class WaypointNavigator : MonoBehaviour
    {
        PedestrianController controller;
        public Waypoint currentWaypoint;

        int direction;

        private void Awake()
        {
            controller = GetComponent<PedestrianController>();
        }

        // Start is called before the first frame update
        void Start()
        {
            direction = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
            controller.SetDestination(currentWaypoint.GetPosition());
        }

        // Update is called once per frame
        void Update()
        {
            if (controller.reachedDestination)
            {
                bool shouldBranch = false;

                if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
                {
                    shouldBranch = UnityEngine.Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
                }

                if (shouldBranch)
                {
                    currentWaypoint = currentWaypoint.branches[UnityEngine.Random.Range(0, currentWaypoint.branches.Count - 1)];
                }
                else
                {
                    if (direction == 0)
                    {
                        if (currentWaypoint.nextWaypoint != null)
                        {
                            currentWaypoint = currentWaypoint.nextWaypoint;
                        }
                        else
                        {
                            currentWaypoint = currentWaypoint.previousWaypoint;
                            direction = 1;
                        }
                    }
                    else if (direction == 1)
                    {
                        if (currentWaypoint.previousWaypoint != null)
                        {
                            currentWaypoint = currentWaypoint.previousWaypoint;
                        }
                        else
                        {
                            currentWaypoint = currentWaypoint.nextWaypoint;
                            direction = 0;
                        }
                    }
                }

                controller.SetDestination(currentWaypoint.GetPosition());
            }
        }
    }
}
