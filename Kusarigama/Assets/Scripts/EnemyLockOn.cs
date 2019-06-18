using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{

    public GameObject playerCamera;
    public GameObject lockOnCamera;


    void Update()
    {
        if(Input.GetButtonDown("LockOn") && playerCamera.activeSelf == true)
        {
            lockOnCamera.SetActive(true);
            playerCamera.SetActive(false);
        }else if(Input.GetButtonDown("LockOn") && lockOnCamera.activeSelf == true)
        {
            playerCamera.SetActive(true);
            lockOnCamera.SetActive(false);
        }
    }

}
