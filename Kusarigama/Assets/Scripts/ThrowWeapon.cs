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

    float minClamp = -30f;
    float maxClamp = 30f;

    public Transform targetIndicator;
    public Camera mainCam;

    void Start()
    {
        charMovement = GetComponent<CharMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        upDown = Mathf.Clamp(upDown - Input.GetAxis("Mouse Y"),minClamp,maxClamp);
        leftRight = Mathf.Clamp(leftRight - Input.GetAxis("Mouse X"),minClamp,maxClamp);

        if (Input.GetButton("Fire2"))
        {
            //Aim
            AimCamActive();
            targetIndicator.gameObject.SetActive(true);
            aiming = true;
            MoveAimingTarget();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            targetIndicator.gameObject.SetActive(false);
            PlayerCamActive();
            aiming = false;
        }
    }

    void MoveAimingTarget()
    {
        aimingTargetRotation.localEulerAngles = new Vector3(upDown,-leftRight,0);
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
