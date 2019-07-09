using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Editor;

public class EnemyLockOn : MonoBehaviour
{
    public Transform player;
    //Cameras
    public GameObject playerCamera;
    public GameObject lockOnCamera;
    public GameObject mainCamera;
    //Enemies
    public GameObject[] enemies;
    public GameObject closestEnemy;  
    //Raycast
    public bool enemyObstructed;
    //Target
    public Cinemachine.CinemachineTargetGroup group;
    public Cinemachine.CinemachineTargetGroup.Target[] targets;
    public Transform targetIndicator;
    //Timer
    public float waitForUnLock;
    public bool waited;

    private void Start()
    {
        targets = group.m_Targets;
        waitForUnLock = 0.3f;
    }

    void Update()
    {
        //Press the Button to Lock on 
        if (Input.GetButtonDown("LockOn") && playerCamera.activeSelf == true && enemyObstructed == false)
        {
            ActivateLockOncam();
            FindClosestEnemy();
            targetIndicator.gameObject.SetActive(true);
        }
        else if (Input.GetButtonDown("LockOn") && lockOnCamera.activeSelf == true || closestEnemy == null || enemyObstructed == true)
        {
            ActivatePlayerCam();
            targetIndicator.gameObject.SetActive(false);
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
                    StartCoroutine(WaitForUnLock());
                }
            }
        }

        //TargetIndicator
        if (closestEnemy != null)
        {
            targetIndicator.transform.position = new Vector3(closestEnemy.transform.position.x,closestEnemy.transform.position.y+1,closestEnemy.transform.position.z);
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

    IEnumerator WaitForUnLock()
    {
        yield return new WaitForSeconds(waitForUnLock);
        enemyObstructed = true;
    }
}
