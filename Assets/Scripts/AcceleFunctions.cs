using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AcceleFunctions
{
    /* This program used to implement the functions of Acceleration.cs */
    public static string ConvertYPositionToSignal(Vector3 previousPosition, ref Vector3 currentPosition, ref string ySignal)
    {
        if (previousPosition.y < currentPosition.y)
        {
            ySignal = "0"; // 棒棒向下
        }
        else if(previousPosition.y >= currentPosition.y)
        {
            ySignal = "1"; // 棒棒向上
        }

        currentPosition.y = previousPosition.y;

        return ySignal;
    }

    public static string ConvertAccelerationToSignal(Vector3 previousAcceleration, float currentAccelerationX, float currentAccelerationZ, ref string[] signal, ref string state)
    {
        if (previousAcceleration.x < currentAccelerationX)
        {
            signal[2] = "0"; // 棒棒向下
        }
        else //if(previousAcceleration.x >= currentAccelerationX)
        {
            signal[2] = "1"; // 棒棒向上
        }

        currentAccelerationX = previousAcceleration.x;
        
        return signal[2];
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
