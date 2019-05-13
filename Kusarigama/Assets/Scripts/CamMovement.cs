using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour {

    public Transform LookAt;
    public Transform camTransform;
    public float rotationSpeed = 2.0f;
    public Vector3 characterPosition;
    public Vector3 cameraPosition;

    private Camera cam;

    public float distance = 7.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    

    private const float X_ANGLE_MIN = 5.0f;
    private const float X_ANGLE_MAX = 40.0f;
 
    // Use this for initialization
    void Start ()
    {
        camTransform = transform;
        cam = Camera.main;
	}
  

    void LateUpdate()
    {
        currentX -= Input.GetAxis("VerticalTurn");
        currentY += Input.GetAxis("HorizontalTurn");
        currentX = Mathf.Clamp(currentX, X_ANGLE_MIN, X_ANGLE_MAX);
        characterPosition = new Vector3(LookAt.position.x, LookAt.position.y, LookAt.position.z);
        cameraPosition = new Vector3(camTransform.position.x, camTransform.position.y, camTransform.position.z);

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentX * rotationSpeed, currentY * rotationSpeed, 0);
       
        camTransform.position = Vector3.Lerp(camTransform.position, LookAt.position + rotation * dir,5f);
        
        camTransform.LookAt(LookAt.position);

    }
}
