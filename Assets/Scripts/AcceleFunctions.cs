using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AcceleFunctions
{
    /* This program used to implement the functions of Acceleration.cs */
    public static string ConvertPositionToSignal(Vector3 previousPosition,
                                                 ref Vector3 currentPosition,
                                                 ref string ySignal,
                                                 Vector3 previousAcceleration,
                                                 ref float currentAccelerationX,
                                                 ref float currentAccelerationZ)
    {
        /*
        case 0 : reset to same heigh (2.5s)
         case 1 : both up with normal speed (0.5s)
         case 2 : both down with normal speed (0.5s) 
         case 3 : both up with slow speed (0.1s + 0.1s delay) * 2 times + 0.1s
         case 4 : turn left (0.5s)
         case 5 : turn right (0.5s)
        */
        if ((previousPosition.y - currentPosition.y) < 0
            && (previousAcceleration.x - currentAccelerationX) < 0.8
            && (previousAcceleration.z - currentAccelerationZ) < 0.8)
        {
            ySignal = "2";
        }
        else if ((previousPosition.y - currentPosition.y) > 0
                && (previousAcceleration.x - currentAccelerationX) < 0.8
                && (previousAcceleration.z - currentAccelerationZ) < 0.8)
        {
            ySignal = "1";
        }
        else if ((previousAcceleration.z - currentAccelerationZ) < 0)
        {
            ySignal = "4";
        }
        else if ((previousAcceleration.z - currentAccelerationZ) > 0)
        {
            ySignal = "5";
        }
        currentPosition.y = previousPosition.y;
        currentAccelerationX = previousAcceleration.x;
        currentAccelerationZ = previousAcceleration.z;

        return ySignal;
    }
}
