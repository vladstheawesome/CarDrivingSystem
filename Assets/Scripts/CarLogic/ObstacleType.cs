using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarGame.CarLogic
{
    public enum ObstacleType
    {
        Vehicle,
        Pedestration,
        RoadObject,
        TrafficLightAhead,
        TrafficLight,
        RoadSignSpeedLimit
    }

    public enum SpeedLimit
    {
        Slow = 25,
        Medium = 50,
        Fast = 100,
        NA,
    }
}
