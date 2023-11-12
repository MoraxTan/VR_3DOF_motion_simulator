using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class Signal
{
    public enum rotateStatus
    {
        Left, Right, Default
    }

    public enum posStatus
    {
        Up, Down, Default
    }

    private rotateStatus rotateSignal = rotateStatus.Default;

    private posStatus posSignal = posStatus.Default;
    public Signal() { }
    public void setX(rotateStatus x)
    {
        this.rotateSignal = x;
    }

    public void setY(posStatus y)
    {
        this.posSignal = y;
    }

    public string toString()
    {
        return "{rotateSignal: " + this.rotateSignal + " ,posSignal: " + this.posSignal + " }";
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
            outputSignal.setX(Signal.rotateStatus.Right);
        }
        else if (eulerRotationY <= 90f - rotateGate)
        {
            outputSignal.setX(Signal.rotateStatus.Left);
        }

        // Check for upward/downward movement
        if ((previousPosition.y - olderPositionY) <= -0.5)
        {
            outputSignal.setY(Signal.posStatus.Down);
        }
        else if ((previousPosition.y - olderPositionY) >= 0.5)
        {
            outputSignal.setY(Signal.posStatus.Up);
        }
        olderPositionY = previousPosition.y;

        return outputSignal.toString();
    }
}