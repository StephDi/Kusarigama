﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharMovement : MonoBehaviour {

   
    //Forces
    public float dashForce;
    public float jumpForce;
    //Animation
    public Animator anim;
    //Movement
    public float moveSpeed = 10f;
    public float rotateSpeed = 3f;
    float horizontal;
    float vertical;
    float leftRigh_RightJoyStick;
    public Vector3 movement;
    public Vector3 turnVector;
    public bool dashPossible;
    //falling
    public float fallMultiplier = 2.5f;
    //other
    public Transform cam;
    public Cinemachine.CinemachineVirtualCamera LockOnCam;
    public Rigidbody character;
    private float groundDistance = 0.1f;
    public LayerMask layerMask;
    public AimWeapon aimWeapon;
    public RangedCombatGhost rangedCombatGhost;
    public RangedCombat rangedCombat;
    public float dashCoolDown;
    private Vector3 raycastPoint;

    void Start ()
    {
        character = GetComponent<Rigidbody>();
        aimWeapon = GetComponent<AimWeapon>();
        rangedCombatGhost = GetComponent<RangedCombatGhost>();
        rangedCombat = GetComponent<RangedCombat>();

        dashPossible = true;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        leftRigh_RightJoyStick = Input.GetAxis("Mouse X");

        //Jump
        if (canJump())
        {
            anim.SetBool("jump",false);
            anim.SetBool("grounded", true);
            if (Input.GetButtonDown("Jump")) 
            {
                anim.SetBool("jump", true);
                anim.SetBool("grounded",false);
            }
        }     
        else if (!canJump())
        {
            anim.SetBool("jump",false);
            anim.SetBool("grounded",false);
        }

        StopSliding();
    }

    void FixedUpdate ()
    {
        if (character.velocity.y < 0)
        {
            character.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        if (canJump())
        {
            Vector3 antiGravity = Physics.gravity * character.mass;
            character.AddForce(-antiGravity);
        }

        //Cameramovement
        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        //Move
        if (rangedCombat.rangedAttackTrigger == 0)
        {
            if (rangedCombatGhost.rangedAttackTriggerGhost == 0)
            {
                if (!AnimationIsPlaying(anim, "Armature_Rundumschlag_Ghost"))
                {
                    if (!AnimationIsPlaying(anim, "Armature_Nahkampfschlag_Ranged"))
                    {
                        if (!AnimationIsPlaying(anim, "Armature_Nahkampfschlag_von_links_Ranged"))
                        {
                            if (!AnimationIsPlaying(anim, "Armature_Rundumschlag_mit_rechts_ranged"))
                            {
                                movement = new Vector3(horizontal, 0, vertical);
                                character.MovePosition(transform.position + (camF * movement.z + camR * movement.x) * moveSpeed * Time.fixedDeltaTime);
                            }                          
                        }
                    }
                }
            }
        }
       
        //Turn Character
        if (aimWeapon.aiming == false)
        {
            if (LockOnCam.Priority == 0)
            {
                if (movement != Vector3.zero)
                {
                    character.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.x * camR + movement.z * camF), 0.35F));
                }
            }
        }

        //Turn while Aiming
        if (aimWeapon.aiming == true)
        {
            turnVector = new Vector3(0,leftRigh_RightJoyStick,0);
            turnVector = turnVector.normalized * rotateSpeed;
            if (aimWeapon.leftRight > 14f)
            {
                //Turn left
                Quaternion deltaRotation = Quaternion.Euler(turnVector);
                character.MoveRotation(Quaternion.Slerp(character.rotation, character.rotation * deltaRotation, 0.35f));
            }
            if (aimWeapon.leftRight < -14f)
            {
                //Turn right
                Quaternion deltaRotation = Quaternion.Euler(turnVector);
                character.MoveRotation(Quaternion.Slerp(character.rotation, character.rotation * deltaRotation, 0.35f));
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

        //dash
        if (Input.GetButtonDown("Dash") && movement.sqrMagnitude != 0 && dashPossible == true)
        {
            StartCoroutine(DashCoolDown());
            dashPossible = false;
            anim.SetTrigger("dash");
        }
        if (dashPossible == false)
        {
            character.MovePosition(character.position + (movement.z * camF + movement.x * camR) * dashForce * Time.fixedDeltaTime);
        }
    }


    //Check if the Animation "stateMame" is Playing
    bool AnimationIsPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }

    //StopSlidingOnSlpoes
    void StopSliding()
    {
        if (movement == new Vector3(0, 0, 0) && dashPossible == true)
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

    private bool canJump()
    {
        RaycastHit jumpHit1;
        RaycastHit jumpHit2;
        RaycastHit jumpHit3;
        RaycastHit jumpHit4;
        RaycastHit jumpHit5;
        Physics.Raycast (transform.position + Vector3.forward * .5f, Vector3.down, out jumpHit1, groundDistance, layerMask);
        Physics.Raycast(transform.position + Vector3.back * .5f, Vector3.down, out jumpHit2, groundDistance, layerMask);
        Physics.Raycast(transform.position + Vector3.left * .5f, Vector3.down, out jumpHit3, groundDistance, layerMask);
        Physics.Raycast(transform.position + Vector3.right * .5f, Vector3.down, out jumpHit4, groundDistance, layerMask);
        Physics.Raycast(transform.position, Vector3.down, out jumpHit5, groundDistance, layerMask);
        return jumpHit1.collider != null || jumpHit2.collider != null|| jumpHit3.collider != null|| jumpHit4.collider != null|| jumpHit5.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + Vector3.forward * .5f, Vector3.down * groundDistance);
        Gizmos.DrawRay(transform.position + Vector3.back * .5f, Vector3.down * groundDistance);
        Gizmos.DrawRay(transform.position + Vector3.left * .5f, Vector3.down * groundDistance);
        Gizmos.DrawRay(transform.position + Vector3.right * .5f, Vector3.down * groundDistance);
        Gizmos.DrawRay(transform.position, Vector3.down * groundDistance);
    }

    IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(dashCoolDown);
        dashPossible = true;
    }
}
