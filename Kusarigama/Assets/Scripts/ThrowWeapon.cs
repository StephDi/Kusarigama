using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    public Cinemachine.CinemachineFreeLook playerCam;
    public Cinemachine.CinemachineFreeLook aimCam;

    public Transform aimingTargetRotation;
    public Transform aimingTarget;
    public Rigidbody character;
    public Transform weapon;
    public Transform weaponPoint;

    public CharMovement charMovement;

    public Transform targetIndicator;
    public Camera mainCam;

    public Animator anim;

    float upDown;
    float leftRight;
    float throwValue;
    public bool aiming;
    bool throwing = false;
    bool weaponIsFlying;

    float minClamp = -30f;
    float maxClamp = 30f; 

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
            charMovement.moveSpeed = 5f;
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
            charMovement.moveSpeed = 10f;
            anim.speed = 1;
            anim.SetBool("aiming",false);
            ResetAimingPoint();
            if (weaponIsFlying == false)
            {
                ResetWeapon();
            }
        }

        ThrowWeaponForward();
        PullWeaponBack();
    }

    void ResetAimingPoint()
    {
        aimingTargetRotation.localRotation = Quaternion.identity;
        upDown = 0f;
        leftRight = 0f;
    }

    public void ResetWeapon()
    {
        weapon.gameObject.SetActive(true);
        weapon.SetParent(weaponPoint);

        weapon.localPosition = Vector3.zero;
        weapon.localRotation = Quaternion.identity;
        //transform.localScale = Vector3.one;      

        weaponIsFlying = false;
        anim.SetBool("weaponIsFlying", false);
    }

    void PullWeaponBack()
    {
        if (throwing == false && weaponIsFlying == true)
        {
            //Weapon.SetParent(WeaponPoint);
            weapon.rotation = weaponPoint.rotation;
            weapon.position = Vector3.Lerp(weapon.position,weaponPoint.position,0.1f);           
        }
    }

    void ThrowWeaponForward()
    {
        if (throwing == true)
        {
            weaponIsFlying = true;
            anim.SetBool("weaponIsFlying",true);
            weapon.SetParent(null);
            weapon.position = Vector3.Lerp(weapon.position, aimingTarget.position, 0.1f);
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
        //if (aimingTargetRotation.localEulerAngles.y < -30 || aimingTargetRotation.localEulerAngles.y > 30)
        //{
        //    character.MoveRotation(Quaternion.Euler(0,-leftRight,0));
        //}
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
