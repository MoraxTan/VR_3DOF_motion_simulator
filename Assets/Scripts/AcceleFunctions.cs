using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AcceleFunctions
{
    /* This program used to implement the functions of Acceleration.cs */
    public static string ConvertYPositionToSignal(Vector3 previousPosition, float currentPositionY, ref string[] signal, ref string state)
    {
        if (previousPosition.y > currentPositionY)
        {
            signal[2] = "0"; // 向下
        }
        else //if(previousAcceleration.y == currentAccelerationY)
        {
            signal[2] = "1"; // 向上
        }

        currentPositionY = previousPosition.y;

        return signal[2];
    }

    public static string ConvertAccelerationToSignal(Vector3 previousAcceleration, float currentAccelerationX, float currentAccelerationZ, ref string[] signal, ref string state)
    {
        if (previousAcceleration.x > currentAccelerationX)
        {
            signal[2] = "0"; // 向下
        }
        else //if(previousAcceleration.x == currentAccelerationX)
        {
            signal[2] = "1"; // 向上
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
