using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acceleration : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 previousPosition;
    private Vector3 previousVelocity;
    private Vector3 previousAcceleration;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousPosition = rb.position;
        previousVelocity = Vector3.zero;
        previousAcceleration = Vector3.zero;
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
    }
}
