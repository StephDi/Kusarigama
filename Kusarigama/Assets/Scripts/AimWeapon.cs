using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    //Cam
    public Cinemachine.CinemachineFreeLook playerCam;
    public Cinemachine.CinemachineFreeLook aimCam;
    public Cinemachine.CinemachineVirtualCamera lockOnCam;
    //Aim Transforms
    public Transform TargetIndicator;
    public Transform AimingIndicator;
    public Transform aimingTargetRotation;   
    //Animator
    public Animator anim;  
    //Inputs
    float upDown;
    public float leftRight;
    //Clamprestrictions
    float minClampLeftRight = -15f;
    float maxClampLeftRight = 15f;
    float minClampUpDown = -30f;
    float maxClampUpDown = 30f;
    //other
    public bool aiming;
    public CharMovement charMovement;
    private float camSwitchTime;
    bool LockOnWasActive;

    // Start is called before the first frame update
    void Start()
    {
        charMovement = GetComponent<CharMovement>();

        camSwitchTime = 0.35f;
        LockOnWasActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Inputs
        upDown = Mathf.Clamp(upDown - Input.GetAxis("Mouse Y"), minClampUpDown, maxClampUpDown);
        leftRight = Mathf.Clamp(leftRight - Input.GetAxis("Mouse X"), minClampLeftRight, maxClampLeftRight);

        if (Input.GetButtonDown("Fire2"))
        {
            if (lockOnCam.Priority == 1)
            {
                LockOnWasActive = true;
            }
            else if (lockOnCam.Priority == 0)
            {
                LockOnWasActive = false;
            }
        }

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
            if (LockOnWasActive == true)
            {
                LockOnCamActive();
            }
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
        StartCoroutine(WaitForCamswitch());
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
        aimingTargetRotation.localEulerAngles = new Vector3(upDown, -leftRight, 0);
    }

    void AimCamActive()
    {
        aimCam.Priority = 1;
        lockOnCam.Priority = 0;
        playerCam.Priority = 0;
        TargetIndicator.gameObject.SetActive(false);
    }

    void PlayerCamActive()
    {
        playerCam.Priority = 1;
        aimCam.Priority = 0;
        lockOnCam.Priority = 0;
        TargetIndicator.gameObject.SetActive(false);
    }

    void LockOnCamActive()
    {       
        lockOnCam.Priority = 1;
        playerCam.Priority = 0;
        aimCam.Priority = 0;
        TargetIndicator.gameObject.SetActive(true);
    }

    //Stop Turning during Camswitch -> CharMovement
    IEnumerator WaitForCamswitch()
    {
        yield return new WaitForSeconds(camSwitchTime);
        aiming = false;
    }

}
