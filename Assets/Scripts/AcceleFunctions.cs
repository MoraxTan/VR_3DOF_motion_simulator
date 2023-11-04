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
                                                 ref float currentAccelerationZ,)
    {
        /*
         * case 1: up
         * case 2: down
         * case 3: uphill dan turn left
         * case 4: uphill dan turn right
         * case 5: downhill dan turn left
         * case 6: downhill dan turn right
         */
        if ((previousPosition.y - currentPosition.y) < 0 
            && (previousAcceleration.x - currentAccelerationX) < 0.5
            && (previousAcceleration.z - currentAccelerationZ) < 0.5)
        {
            ySignal = "2";
        }
        else if((previousPosition.y - currentPosition.y) > 0 
                && (previousAcceleration.x - currentAccelerationX) < 0.5
                && (previousAcceleration.z - currentAccelerationZ) < 0.5)
        {
            ySignal = "1";
        }
        else if((previousAcceleration.x - currentAccelerationX) > 0
                && (previousAcceleration.z - currentAccelerationZ) < 0)
        {
            ySignal = "3"; 
        }
        else if((previousAcceleration.x - currentAccelerationX) > 0
                && (previousAcceleration.z - currentAccelerationZ) > 0)
        {
            ySignal = "4";
        }
        else if((previousAcceleration.x - currentAccelerationX) < 0
                && (previousAcceleration.z - currentAccelerationZ) < 0)
        {
            ySignal = "5";
        }
        else if((previousAcceleration.x - currentAccelerationX) < 0
                && (previousAcceleration.z - currentAccelerationZ) > 0)
        {
            ySignal = "6";
        }
        currentPosition.y = previousPosition.y;
        currentAccelerationX = previousAcceleration.x;
        currentAccelerationZ = previousAcceleration.z;

        return ySignal;
    }

    public static bool IsSame(string[] signal)
    {
        if (signal[0] == signal[2] && signal[1] == signal[2])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string CheckEnd(Vector3 position, ref float[] position_x, ref float[] position_y, ref float[] position_z)
    {
        // 检查游戏是否结束的代码
        // 当position.x/y/z连续重复时，结束游戏
        // 遇到问题：position进行x/y/z比对，同一数据下不会return "stop"
        position_x[1] = position.x;
        position_y[1] = position.y;
        position_z[1] = position.z;

        if (position_x[0] == position_x[1] && position_y[0] == position_y[1] && position_z[0] == position_z[1])
        {
            return "stop";
        }
        else
        {
            position_x[0] = position_x[1];
            position_y[0] = position_y[1];
            position_z[0] = position_z[1];
        }
        return null;
    }
}