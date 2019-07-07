using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Editor;

public class EnemyLockOn : MonoBehaviour
{
    public Transform player;
    public GameObject playerCamera;
    public GameObject lockOnCamera;
    public GameObject mainCamera;
    public GameObject[] enemies;
    public GameObject closestEnemy;
    public Cinemachine.CinemachineTargetGroup group;
    public Cinemachine.CinemachineTargetGroup.Target[] targets;
    public bool enemyObstructed;

    private void Start()
    {
        targets = group.m_Targets;
    }

    void Update()
    {

        if (Input.GetButtonDown("LockOn") && playerCamera.activeSelf == true && enemyObstructed == false)
        {
            ActivateLockOncam();
            FindClosestEnemy();
        }
        else if (Input.GetButtonDown("LockOn") && lockOnCamera.activeSelf == true)
        {
            ActivatePlayerCam();
        }

        //Test if you can see the Enemy
        RaycastHit hit;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        if (closestEnemy != null)
        { 
            if (Physics.Raycast(mainCamera.transform.position, -(mainCamera.transform.position - closestEnemy.transform.position), out hit, Mathf.Infinity, layerMask))
            {
                //RayGizmo
                //Debug.DrawRay(mainCamera.transform.position, -(mainCamera.transform.position - closestEnemy.transform.position), Color.red);
                //print(hit.collider);

                if (hit.collider.tag == "Enemy")
                {
                    enemyObstructed = false;
                }
                else
                {
                    enemyObstructed = true;
                }
            }
        }
    }

    void FindClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var distance = Mathf.Infinity;
        var position = player.transform.position;
        foreach (GameObject e in enemies)
        {
            var diff = (e.transform.position - position);
            var curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closestEnemy = e;
                distance = curDistance;
               
                if (curDistance < 1000f)
                {  
                    targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
                    targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = closestEnemy.transform, radius = 4.0f, weight = 1.0f };                 
                }
                else if (curDistance > 1000f)
                {
                    targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
                    targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = null, radius = 4.0f, weight = 1.0f };
                    ActivatePlayerCam();
                }

            }
        }
    }

    void ActivateLockOncam()
    {
        lockOnCamera.SetActive(true);
        playerCamera.SetActive(false);
    }

    void ActivatePlayerCam()
    {
        playerCamera.SetActive(true);
        lockOnCamera.SetActive(false);
    }
}
