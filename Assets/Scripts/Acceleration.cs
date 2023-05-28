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
        arduinoPort.Write(signal);

        // 调试信息
        Debug.Log("Signal: " + signal);
    }

    // 关闭串口连接
    private void OnDestroy()
    {
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }

    // 将加速度转换为信号形式
    private string ConvertAccelerationToSignal(Vector3 acceleration)
    {
        // 根据加速度值设置对应的信号
        string signal = "";

        // 根据实际需求设置阈值
        float threshold = 0.5f;

        if (acceleration.x > threshold)
        {
            signal += "1"; // 向右
        }
        else if (acceleration.x < -threshold)
        {
            signal += "2"; // 向左
        }
        else
        {
            signal += "0"; // 停止
        }

        if (acceleration.z > threshold)
        {
            signal += "1"; // 向前
        }
        else if (acceleration.z < -threshold)
        {
            signal += "2"; // 向后
        }
        else
        {
            signal += "0"; // 停止
        }

        return signal;
    }
}
