using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour {

    public Transform LookAt;
    public Transform camTransform;
    public float rotationSpeed = 2.0f;

    private Camera cam;

    private float distance = 12.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    

    private const float Y_ANGLE_MIN = 5.0f;
    private const float Y_ANGLE_MAX = 70.0f;

    // Use this for initialization
    void Start ()
    {
        camTransform = transform;
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentX -= Input.GetAxis("VerticalTurn");
        currentY += Input.GetAxis("HorizontalTurn");

        currentX = Mathf.Clamp(currentX,Y_ANGLE_MIN,Y_ANGLE_MAX);
    }

    void FixedUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentX * rotationSpeed, currentY * rotationSpeed, 0);
        camTransform.position = LookAt.position + rotation * dir;
        camTransform.LookAt(LookAt.position);
    }
}
