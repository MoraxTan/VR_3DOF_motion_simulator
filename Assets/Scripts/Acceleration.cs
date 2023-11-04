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
    public Vector3 previousPosition;   
    public Vector3 previousVelocity;
    public Vector3 previousAcceleration;
    
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

        yPosSignal = ConvertPositionToSignal(previousPosition, previousAcceleration);

        if (!IsSame(pre))
        {
            Debug.Log("Signal: " + yPosSignal);
            arduinoPort.Write(yPosSignal);
        }

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

    Vector3 currentPosition;        // to save the last state of y, cause the change of y is simplify to use

    /* only for the y position */
    private string ConvertPositionToSignal(Vector3 previousPosition, Vector3 previousAcceleration)
    {
        return AcceleFunctions.ConvertPositionToSignal(previousPosition,
                                                       ref currentPosition,
                                                       ref ySignal,
                                                       previousAcceleration,
                                                       ref currentAccelerationX,
                                                       ref currentAccelerationZ);
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
