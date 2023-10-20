using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class Acceleration : MonoBehaviour
{   
    // for rigid body use
    private Rigidbody rb;   

    // to save the last state
    private Vector3 previousPosition;   
    private Vector3 previousVelocity;
    private Vector3 previousAcceleration;
    
    // determine serial port 
    private SerialPort arduinoPort; 
    // define your port name
    public string portName = "COM3";
    // baud rate setting, default num '9600'
    public int baudRate = 9600; 

    // Start is called before the first frame update
    void Start()
    {
        // define the component of rigid body
        rb = GetComponent<Rigidbody>();

        // define the begin number to these parameters
        previousPosition = rb.position;
        previousVelocity = Vector3.zero;
        previousAcceleration = Vector3.zero;

        // initialize and open to use the serial port
        arduinoPort = new SerialPort(portName, baudRate);
        arduinoPort.Open();
    }

    // just use Update, not a FixedUpdate. We only need called once per frame, dont want more
    void Update()
    {
        // record the time per frame
        float timeDelta = Time.deltaTime;

        // calculate the velocity and acceleration of rigid body
        Vector3 velocity = (rb.position - previousPosition) / timeDelta;
        Vector3 acceleration = (velocity - previousVelocity) / timeDelta;

        // asign the parameters
        previousVelocity = velocity;
        previousPosition = rb.position;
        previousAcceleration = acceleration;

        //rb.AddForce(previousAcceleration, ForceMode.Acceleration);
        string[] pre = new string[3];
        string yPosSignal = "";
        pre[2] = ConvertAccelerationToSignal(previousAcceleration);

        yPosSignal = ConvertYPositionToSignal(previousPosition);

        /*if (CheckEnd(rb.position) == "stop")
        {
            signal = "stop";
        }*/

        if (!IsSame(pre))
        {
            Debug.Log("Signal: " + pre[2]);
            arduinoPort.Write(pre[2]);
            // arduinoPort.Write(yPosSignal);
        }
        pre[0] = pre[1];
        pre[1] = pre[2];
        
        StartCoroutine(delayFunction());
    }

    IEnumerator delayFunction()
    {
        // its better than WaitForSeconds()
        yield return new WaitForSecondsRealtime((float)1.0); 
    }

    private void OnDestroy()
    {
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }

    string[] signal = new string[3];    // for x,z position 
    string ySignal = "0";               // for y position only

    string state = "none";

    float currentAccelerationX = 0f;    // to save the last state of x
    float currentAccelerationZ = 0f;    // to save the last state of z

    float currentPositionY = 0f;        // to save the last state of y, cause the change of y is simplify to use

    /* only for the y position */
    private string ConvertYPositionToSignal(Vector3 previousPosition)
    {
        return AcceleFunctions.ConvertYPositionToSignal(previousPosition, currentPositionY, ref ySignal);
    }

    /* used to compare the change of x and z */
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
