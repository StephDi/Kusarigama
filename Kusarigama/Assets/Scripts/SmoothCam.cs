using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCam : MonoBehaviour {

    public Transform target;

    public float smoothSpeed;
    public Vector3 offset;

    public float Rotationspeed = 3.0f;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("HorizontalTurn") * Rotationspeed, Vector3.up);

        offset = camTurnAngle * offset;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Slerp(transform.position, desiredPosition, smoothSpeed);
        transform.LookAt(target);
    }
}
