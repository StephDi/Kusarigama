using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    public Cinemachine.CinemachineFreeLook playerCam;
    public Cinemachine.CinemachineFreeLook aimCam;

    public bool aiming;

    public Transform aimingTargetRotation;
    public Rigidbody character;

    public CharMovement charMovement;

    float upDown;
    float leftRight;
    float throwValue;
    bool throwing = false;

    float minClamp = -30f;
    float maxClamp = 30f;

    public Transform targetIndicator;
    public Camera mainCam;

    public Animator anim;

    void Start()
    {
        charMovement = GetComponent<CharMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        upDown = Mathf.Clamp(upDown - Input.GetAxis("Mouse Y"),minClamp,maxClamp);
        leftRight = Mathf.Clamp(leftRight - Input.GetAxis("Mouse X"),minClamp,maxClamp);

        throwValue = Input.GetAxis("Throw");

        if (Input.GetButton("Fire2"))
        {
            //Aim
            AimCamActive();
            targetIndicator.gameObject.SetActive(true);
            aiming = true;
            MoveAimingTarget();
            charMovement.moveSpeed = 7f;
            anim.SetBool("aiming",true);
            if (throwValue >= 1 && throwing == false)
            {
                anim.speed = 1;
                throwing = true;
            }
            if (throwValue < 1 && throwing == true)
            {
                anim.speed = 1;
                throwing = false;
            }
        }

        if (Input.GetButtonUp("Fire2"))
        {
            targetIndicator.gameObject.SetActive(false);
            PlayerCamActive();
            aiming = false;
            charMovement.moveSpeed = 15f;
            anim.speed = 1;
            anim.SetBool("aiming",false);
        }
    }

    void StopAnimation()
    {
        anim.speed = 0;
    }

    void MoveAimingTarget()
    {
        aimingTargetRotation.localEulerAngles = new Vector3(upDown,-leftRight,0);
        targetIndicator.gameObject.SetActive(true);
    }

    void PlayerCamActive()
    {
        playerCam.Priority = 1;
        aimCam.Priority = 0;      
    }

    void AimCamActive()
    {
        aimCam.Priority = 1;
        playerCam.Priority = 0;        
    }
}
