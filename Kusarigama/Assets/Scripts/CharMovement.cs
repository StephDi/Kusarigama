using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour {

    public float moveSpeed = 15f;
    public Rigidbody character;
    public float dashForce;
    public bool dashCooldown = true;
    public float dashDuration;
    public float jumpForce;
    public Transform cam;

    // Use this for initialization
    void Start () {
        character = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
    //Move
	void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal * moveSpeed * Time.deltaTime,0, vertical * moveSpeed * Time.deltaTime);

        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        character.MovePosition(transform.position + (camF * movement.z + camR * movement.x));
  
        if (movement != Vector3.zero)
        {
            character.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.x * camR + movement.z * camF), 0.35F));
        }

        //Dash
        if ((Input.GetButtonDown("Dash")) && (dashCooldown == true))
        {
            character.AddForce(movement.normalized * dashForce);
            dashCooldown = false;
            StartCoroutine(Dash());
        }

        //jump
        if (Input.GetButtonDown("Jump") )
        {
            character.AddForce(0,jumpForce,0);
        }


    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashDuration);
        dashCooldown = true;
    }
}
