using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    //Cam
    public Cinemachine.CinemachineFreeLook playerCam;
    public Cinemachine.CinemachineFreeLook aimCam;
    //Aim Transforms
    public Transform AimingIndicator;
    public Transform aimingTargetRotation;   
    //Animator
    public Animator anim;  
    //Inputs
    float upDown;
    float leftRight;
    //Clamprestrictions
    float minClamp = -30f;
    float maxClamp = 30f;
    //other
    public bool aiming;
    public CharMovement charMovement;

    // Start is called before the first frame update
    void Start()
    {
        charMovement = GetComponent<CharMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        upDown = Mathf.Clamp(upDown - Input.GetAxis("Mouse Y"), minClamp, maxClamp);
        leftRight = Mathf.Clamp(leftRight - Input.GetAxis("Mouse X"), minClamp, maxClamp);

        if (Input.GetButton("Fire2"))
        {
            AimCamActive();           
            MoveAimingTarget();
            AimActions();
        }

        if (Input.GetButtonUp("Fire2"))
        {          
            PlayerCamActive();           
            ResetAimingPoint();
            RevertAimActions();
        }
    }

    void AimActions()
    {
        AimingIndicator.gameObject.SetActive(true);
        aiming = true;
        charMovement.moveSpeed = 5f;
        anim.SetBool("aiming", true);
    }

    void RevertAimActions()
    {
        AimingIndicator.gameObject.SetActive(false);
        aiming = false;
        charMovement.moveSpeed = 10f;
        anim.SetBool("aiming", false);
    }

    void MoveAimingTarget()
    {
        aimingTargetRotation.localEulerAngles = new Vector3(upDown, -leftRight, 0);
        AimingIndicator.gameObject.SetActive(true);     
    }

    void ResetAimingPoint()
    {
        aimingTargetRotation.localRotation = Quaternion.identity;
        upDown = 0f;
        leftRight = 0f;
    }

    void AimCamActive()
    {
        aimCam.Priority = 1;
        playerCam.Priority = 0;
    }

    void PlayerCamActive()
    {
        playerCam.Priority = 1;
        aimCam.Priority = 0;
    }

}
