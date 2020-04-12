using System.Collections;
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
    public Vector3 direction;
    public Vector3 turnVector;
    [SerializeField] private bool dashInput;
    RaycastHit sphereCastHit;
    public bool dashPossible;
    //Cam
    Vector3 camF;
    Vector3 camR;
    public Transform cam;
    //falling
    public float fallMultiplier = 2.5f;
    //other
    public Cinemachine.CinemachineVirtualCamera LockOnCam;
    public Rigidbody character;
    private float groundDistance = .6f;
    public LayerMask layerMask;
    public AimWeapon aimWeapon;
    public RangedCombatGhost rangedCombatGhost;
    public RangedCombat rangedCombat;
    public float dashCoolDown;
    private float dashDuration;
    private Vector3 groundNormal;
    public float maxGroundAngle;
    public float groundAngle;

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
        if (Input.GetButtonDown("Dash"))
        {
            dashInput = true;

        }

        movement = new Vector3(-vertical, 0, horizontal);

        StopSliding();
        CalculateGroundAngle();

        //Cameramovement
        camF = cam.forward;
        camR = cam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;        

        if (canJump() && groundAngle <= maxGroundAngle)
        {
            groundNormal = sphereCastHit.normal;
            direction = Vector3.Cross(groundNormal, (camF * movement.z + camR * movement.x));
            if (!AnimationIsPlaying(anim, "Armature_Jump") && dashPossible == false)
            {
                character.velocity = Vector3.zero;
            }
        }
        else if (!canJump() || groundAngle >= maxGroundAngle)
        {
            direction = (camF * -movement.x + camR * movement.z);           
        }

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
    }

    void FixedUpdate ()
    {       
        if (character.velocity.y < 0 && !canJump())
        {
            character.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        //Move
        if (rangedCombat.rangedAttackTrigger == 0)
        {
            if (rangedCombatGhost.rangedAttackTriggerGhost == 0)
            {
                if (!AnimationIsPlaying(anim, "Armature_Nahkampfschlag"))
                {
                    if (!AnimationIsPlaying(anim, "Armature_Nahkampfschlag_von_links"))
                    {
                        if (!AnimationIsPlaying(anim, "Armature_Nahkampfschlag_von_rechts"))
                        {
                            if (!AnimationIsPlaying(anim, "Armature_Rundumschlag_Ghost"))
                            {
                                if (!AnimationIsPlaying(anim, "Armature_Nahkampfschlag_Ranged"))
                                {
                                    if (!AnimationIsPlaying(anim, "Armature_Nahkampfschlag_von_links_Ranged"))
                                    {
                                        if (!AnimationIsPlaying(anim, "Armature_Rundumschlag_mit_rechts_ranged"))
                                        {
                                            if (groundAngle <= maxGroundAngle)
                                            {
                                                character.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Turn Character
        if (aimWeapon.aiming == false)
        {
            if (LockOnCam.Priority == 0)
            {
                if (movement != Vector3.zero)
                {
                    character.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x,0,direction.z)), 0.35F));
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
        if (dashInput && movement.sqrMagnitude != 0 && dashPossible == true)
        {
            dashDuration = .3f;
            dashInput = false;
            dashPossible = false;
            anim.SetTrigger("dash");
            FindObjectOfType<AudioManager>().Play("Dash");
            StartCoroutine(DashCoolDown());
        }
        if (dashPossible == false && dashDuration > 0)
        {
            dashDuration -= Time.deltaTime;
            character.velocity = direction * dashForce;
            dashInput = false;
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

    void CalculateGroundAngle()
    {
        if (!canJump())
        {
            groundAngle = 90;
            return;
        }

        groundAngle = Vector3.Angle(sphereCastHit.normal, transform.forward);
    }

    void Jump()
    {
        character.velocity = new Vector3(0, jumpForce, 0);
        FindObjectOfType<AudioManager>().Play("Jump");
    }

    private bool canJump()
    {           
        Physics.SphereCast(transform.position + Vector3.up,groundDistance, Vector3.down * .5f, out sphereCastHit,groundDistance, layerMask);       
        return sphereCastHit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3.up*.5f), groundDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, direction * 10f);
    }

    IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(dashCoolDown);
        dashPossible = true;
    }
}
