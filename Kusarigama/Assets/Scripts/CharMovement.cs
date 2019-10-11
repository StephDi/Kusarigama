using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharMovement : MonoBehaviour {

   
    //Forces
    public float dashForce;
    public float jumpForce;
    //Animation
    public Transform cam;
    public Animator anim;
    //Movement
    public float moveSpeed = 10f;
    float horizontal;
    float vertical;
    public Vector3 movement;
    bool dashPossible;
    //falling
    public float fallMultiplier = 2.5f;
    //other
    public Rigidbody character;
    public bool isGrounded = false;
    public AimWeapon aimWeapon;
    public float dashCoolDown;

    void Start ()
    {
        character = GetComponent<Rigidbody>();
        aimWeapon = GetComponent<AimWeapon>();

        dashPossible = true;
	}

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate ()
    {     
        if (character.velocity.y < 0)
        {
            character.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        //Cameramovement
        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        //Move
        movement = new Vector3(horizontal, 0, vertical);
        character.MovePosition(transform.position + (camF * movement.z + camR * movement.x) * moveSpeed * Time.fixedDeltaTime);

        //Turn Character
        if (aimWeapon.aiming == false)
        {
            if (movement != Vector3.zero)
            {
                character.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.x * camR + movement.z * camF), 0.35F));
            }
        }

        //Animation
        if (movement.sqrMagnitude != 0)
        {
            anim.SetBool("moving", true);
            anim.SetFloat("moveSpeed", Mathf.Abs(movement.magnitude));
        }
        else
        {
            anim.SetBool("moving", false);
        }

        //jump
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            isGrounded = false;
            anim.SetBool("grounded", false);
            anim.SetTrigger("jump");
        }

        Dash();
        StopSliding();        
    }

    void Dash()
    {
        if (Input.GetButtonDown("Dash") && movement.sqrMagnitude != 0 && dashPossible == true)
        {
            StartCoroutine(DashCoolDown());
            dashPossible = false;
            character.AddRelativeForce(Vector3.forward * dashForce);
            anim.SetTrigger("dash");
        }
    }

    //StopSlidingOnSlpoes
    void StopSliding()
    {
        if (movement == new Vector3(0,0,0) && dashPossible == true)
        {
            character.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            character.constraints = RigidbodyConstraints.None;
            character.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void Jump()
    {
        character.velocity = new Vector3(0, jumpForce, 0);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("grounded",true);           
        }
    }  

    void OnTriggerExit(Collider other)
    {
        isGrounded = false;
        anim.SetBool("grounded", false);
    }

    IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(dashCoolDown);
        dashPossible = true;
    }
}
