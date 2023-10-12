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

        Vector3 velocity = (rb.position - previousPosition) / timeDelta;

        Vector3 acceleration = (velocity - previousVelocity) / timeDelta;

        previousVelocity = velocity;
        previousPosition = rb.position;
        previousAcceleration = acceleration;

        rb.AddForce(previousAcceleration, ForceMode.Acceleration);

        string[] pre = new string[3];
        string yPosSignal = "";
        pre[2]= ConvertAccelerationToSignal(previousAcceleration);
        yPosSignal = ConvertYPositionToSignal(previousPosition);
        /*if (CheckEnd(rb.position) == "stop")
        {
            signal = "stop";
        }*/

        if (!IsSame(pre))
        {
            Debug.Log("Signal: " + pre[2]);
            arduinoPort.Write(pre[2]);
        }
        pre[0] = pre[1];
        pre[1] = pre[2];
        
        // yield return new WaitForSeconds((float)0.5);
    }

    private void OnDestroy()
    {
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }

    string[] signal = new string[3];
    string state = "none";
    float currentAccelerationX = 0f;
    float currentAccelerationZ = 0f;

    float currentPositionY = 0f;

    private string ConvertYPositionToSignal(Vector3 previousPosition)
    {
        return AcceleFunctions.ConvertYPositionToSignal(previousPosition, currentPositionY, ref signal, ref state);
    }

    private string ConvertAccelerationToSignal(Vector3 previousAcceleration)
    {
        return AcceleFunctions.ConvertAccelerationToSignal(previousAcceleration, currentAccelerationX, currentAccelerationZ, ref signal, ref state);
    }

    private bool IsSame(string[] signal)
    {
        return AcceleFunctions.IsSame(signal);
    }

    float[] position_x = new float[2];
    float[] position_y = new float[2];
    float[] position_z = new float[2];

    private string CheckEnd(Vector3 position)
    {
        return AcceleFunctions.CheckEnd(position, ref position_x, ref position_y, ref position_z);
    }
}
