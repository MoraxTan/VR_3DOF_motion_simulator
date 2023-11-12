using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class Signal
{
    public enum Status
    {
        /*
        case 0 : reset to same heigh (2.5s)
        case 1 : both up with normal speed (0.5s)
        case 2 : both down with normal speed (0.5s) 
        case 3 : both up with slow speed (0.1s + 0.1s delay) * 2 times + 0.1s
        case 4 : turn left (0.5s)
        case 5 : turn right (0.5s)
        */
        Left=4, Right=5, Up=1, Down=2, Default=8
    }

    private Status rotateSignal = Status.Default;

    private Status posSignal = Status.Default;
    public Signal() { }
    public void setX(Status x)
    {
        this.rotateSignal = x;
    }

    public void setY(Status y)
    {
        this.posSignal = y;
    }

    public string toString()
    {
        return "{rSignal: " + this.rotateSignal + " ,pSignal: " + this.posSignal + " }";
    }
}

public class Acceleration : MonoBehaviour
{
    public Transform cubeTransform;
    private SerialPort arduinoPort;

    public string portName = "COM3";
    public int baudRate = 9600;

    public Vector3 previousPosition;
    public Vector3 previousVelocity;
    public Vector3 previousAcceleration;

    float olderPositionY = 0f;
    Signal outputSignal = new Signal();
    string ySignal;
    IEnumerator DelayFunction()
    {
        yield return new WaitForSecondsRealtime(1.0f);
    }

    private void OnDestroy()
    {
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }

    void Start()
    {
        transform.position = cubeTransform.position;
        previousPosition = transform.position;

        arduinoPort = new SerialPort(portName, baudRate);
        arduinoPort.Open();
    }

    void Update()
    {
        Quaternion currentRotation = transform.rotation;

        previousPosition = transform.position;

        ySignal = ConvertPositionToSignal(previousPosition, currentRotation);

        Debug.Log("Signal: " + ySignal);
        //arduinoPort.Write(yPosSignal);

        //StartCoroutine(DelayFunction());
    }

    string ConvertPositionToSignal(Vector3 previousPosition, Quaternion currentRotation)
    {
        // Use Euler angles for left/right movement detection
        float eulerRotationY = currentRotation.eulerAngles.y;

        float rotateGate = 5f;
        if (eulerRotationY >= 90f + rotateGate)
        {
            outputSignal.setX(Signal.Status.Right);
        }
        else if (eulerRotationY <= 90f - rotateGate)
        {
            outputSignal.setX(Signal.Status.Left);
        }

        // Check for upward/downward movement
        if ((previousPosition.y - olderPositionY) <= -0.5)
        {
            outputSignal.setY(Signal.Status.Down);
        }
        else if ((previousPosition.y - olderPositionY) >= 0.5)
        {
            outputSignal.setY(Signal.Status.Up);
        }
        olderPositionY = previousPosition.y;

        return outputSignal.toString();
    }
}