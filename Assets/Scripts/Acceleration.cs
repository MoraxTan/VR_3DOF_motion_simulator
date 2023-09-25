using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Acceleration : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 previousPosition;
    private Vector3 previousVelocity;
    private Vector3 previousAcceleration;
    private SerialPort arduinoPort; // 串口对象
    public string portName = "COM3"; // Arduino所连接的串口号
    public int baudRate = 9600; // 串口波特率

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousPosition = rb.position;
        previousVelocity = Vector3.zero;
        previousAcceleration = Vector3.zero;

        // 初始化串口
        arduinoPort = new SerialPort(portName, baudRate);
        arduinoPort.Open();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float timeDelta = Time.fixedDeltaTime;

        // 计算物体的速度
        Vector3 velocity = (rb.position - previousPosition) / timeDelta;

        // 计算物体的加速度
        Vector3 acceleration = (velocity - previousVelocity) / timeDelta;

        // 计算平均加速度
        Vector3 averageAcceleration = (acceleration + previousAcceleration) / 2f;

        // 记录当前速度和位置，以便在下一帧中使用
        previousVelocity = velocity;
        previousPosition = rb.position;
        previousAcceleration = acceleration;

        // 将平均加速度应用于物体
        rb.AddForce(averageAcceleration, ForceMode.Acceleration);
        Debug.Log(averageAcceleration.normalized);

        // 将加速度转换为信号形式并发送给Arduino
        string signal = ConvertAccelerationToSignal(averageAcceleration);
        
        if (CheckEnd(rb.position) == "stop")
        {
            signal = "stop";
        }
        
        //arduinoPort.Write(signal);

        // 调试信息
        Debug.Log("Signal: " + signal);
        // yield return new WaitForSeconds((float)0.5);
    }

    // 关闭串口连接
    private void OnDestroy()
    {
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }

    string[] signal = new string[3];
    string state = "none";
    float currentAccelerationY = 0f;
    // 将加速度转换为信号形式
    private string ConvertAccelerationToSignal(Vector3 acceleration)
    {
        // 根据加速度值设置对应的信号
        // 根据实际需求设置阈值
        // float threshold = 1.0f;
        // 根据 x 轴方向判断
        /*
        if (acceleration.x > threshold)
        {
            signal += "1"; // 向右
        }
        else if (acceleration.x < -threshold)
        {
            signal += "0"; // 向左
        }*/


        // 根据 z 轴方向判断
        if (currentAccelerationY > acceleration.y)
        {
            signal[2] = "1"; // 向前
        }
        /*else if (acceleration.y < -threshold)
        {
            signal += "0"; // 向后
        }*/
        else
        {
            signal[2] = "0"; // 停止
        }

        currentAccelerationY = acceleration.y;

        if (!IsSame(signal))
        {
            state = signal[1];
        }
        signal[0] = signal[1];
        signal[1] = signal[2];
        return state;
    }

    private bool IsSame(string[] signal)
    {
        if (signal[0] == signal[2] || signal[1] == signal[2])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    float[] position_x = new float[2];
    float[] position_y = new float[2];
    float[] position_z = new float[2];

    string CheckEnd(Vector3 position)
    {
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
