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
    public Transform Weapon;
    public Transform WeaponPoint;

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
        }

        ThrowWeaponForward();
        PullWeaponBack();
    }

    public void ResetWeapon()
    {
        Weapon.gameObject.SetActive(true);
        Weapon.SetParent(WeaponPoint);

        Weapon.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity;
        //transform.localScale = Vector3.one;
    }

    void PullWeaponBack()
    {
        if (throwing == false && weaponIsFlying == true)
        {
            //Weapon.SetParent(WeaponPoint);
            Weapon.rotation = WeaponPoint.rotation;
            Weapon.position = Vector3.Lerp(Weapon.position,WeaponPoint.position,0.1f);
        }
    }

    void ThrowWeaponForward()
    {
        if (throwing == true)
        {
            weaponIsFlying = true;
            Weapon.SetParent(null);
            Weapon.position = Vector3.Lerp(Weapon.position, aimingTarget.position, 0.1f);
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
