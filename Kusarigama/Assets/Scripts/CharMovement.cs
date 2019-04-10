using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharMovement : MonoBehaviour {

    public float moveSpeed = 15f;
    public Rigidbody character;
    public float dashForce;
    public bool dashCooldown = true;
    public float dashDuration;
    public float jumpForce;
    public Transform cam;
    public Animator anim;
    public bool isGrounded = false;

    // Use this for initialization
    void Start () {
        character = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Move
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal * moveSpeed * Time.deltaTime,0, vertical * moveSpeed * Time.deltaTime);       

        //Animation
        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) != 0)
        {
            anim.SetBool("Moving", true);
            anim.SetFloat("Movespeed", Mathf.Abs(movement.magnitude * 5));
        }
        else
        {
            anim.SetBool("Moving", false);
        }

        //Cameramovement
        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        character.MovePosition(transform.position + (camF * movement.z + camR * movement.x));
        
        //Turn Character
        if (movement != Vector3.zero)
        {
            character.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.x * camR + movement.z * camF), 0.35F));
        }

        //Dash
        if ((Input.GetButtonDown("Dash")) && (dashCooldown == true) && (Mathf.Abs(horizontal) + Mathf.Abs(vertical) != 0))
        {
            character.AddRelativeForce(Vector3.forward * dashForce);
            dashCooldown = false;
            anim.SetBool("Dash",true);
            StartCoroutine(Dash());
        }

        //jump
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            character.velocity = new Vector3(0, jumpForce, 0);
            anim.SetBool("Jump", true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("Grounded",true);
            anim.SetBool("Jump", false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        isGrounded = false;
        anim.SetBool("Grounded", false);
        anim.SetBool("Jump", true);
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashDuration);
        dashCooldown = true;
        anim.SetBool("Dash",false);        
        character.velocity = new Vector3(0, 0, 0);
    }
}
