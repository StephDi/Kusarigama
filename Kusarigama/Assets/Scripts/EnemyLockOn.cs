﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    public Transform player;
    public float playerWeight;
    //Cameras
    public GameObject playerCamera;
    public GameObject lockOnCamera;
    public Camera mainCamera;
    public Cinemachine.CinemachineFreeLook playerCam;
    public Cinemachine.CinemachineVirtualCamera lockOnCam;
    private Cinemachine.CinemachineCollider lockOnCamCollider;
    //Enemies
    private GameObject[] enemies;
    public GameObject closestEnemy;
    public GameObject closestChangeEnemy;
    //Raycast
    RaycastHit enemyRayHit;
    RaycastHit playerRayHit;
    public LayerMask enemyRayLayerMask;
    public LayerMask playerRayLayerMask;
    //Target
    public Cinemachine.CinemachineTargetGroup group;
    public Cinemachine.CinemachineTargetGroup.Target[] targets;
    public Transform targetIndicator;
    public Vector3 enemyPos;
    public float changeTarget;
    public bool changedTarget;
    //Timer
    public float obstructionTime;


    private void Start()
    {
        lockOnCamCollider = lockOnCam.GetComponent<Cinemachine.CinemachineCollider>();
        lockOnCamCollider.enabled = false;
        obstructionTime = 0;
        targets = group.m_Targets;
        closestEnemy = null;
        changedTarget = false;
    }

    void Update()
    {
        changeTarget = Input.GetAxis("Mouse X");

        //Press the Button to Lock on 
        if (Input.GetButtonDown("LockOn") && lockOnCam.Priority == 0)
        {
            FindClosestEnemy();
            targetIndicator.gameObject.SetActive(true);
            //set the Targetindicator every 0.005s - Invokes void TargetIndicator()
            InvokeRepeating("TargetIndicator",0,0.005f);
        }
        else if (Input.GetButtonDown("LockOn") && lockOnCam.Priority == 1 || closestEnemy == null)
        {
            obstructionTime = 0f;
            ActivatePlayerCam();
            targetIndicator.gameObject.SetActive(false);
            targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = playerWeight };
            targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = null, radius = 4.0f, weight = 1.0f };
        }

        //changeTarget reset
        if (changeTarget != 0 && lockOnCam.Priority == 1 && changedTarget == false)
        {
            changedTarget = true;
            ChangeTarget();
        }
        if (changeTarget == 0)
        {
            changedTarget = false;
        }

        //Look at Target
        if (lockOnCam.Priority == 1)
        {
            LookAtEnemy();
        }

        if (closestEnemy != null)
        {
            //TargetObstructionTimer       
            if (TargetObstructed(closestEnemy))
            {
                obstructionTime += Time.deltaTime;
            }
            else if (!TargetObstructed(closestEnemy) && obstructionTime != 0f)
            {
                obstructionTime = 0f;
            }

            //PlayerObstructed -> activate lockoncamCollider
            if (PlayerObstructed())
            {
                lockOnCamCollider.enabled = false;
            }
            else if (!PlayerObstructed())
            {
                lockOnCamCollider.enabled = true;
            }

            //check if Distance to Targeted Enemy gets too big || if Tagret gets obstructed for too long
            if (Vector3.Distance(closestEnemy.transform.position, player.position) >= 35f || TargetObstructed(closestEnemy) && obstructionTime >= 1.5f)
            {
                closestEnemy = null;
                ActivatePlayerCam();
            }
        }
    }

    void FindClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");      
        var position = player.transform.position;
        var dot = Mathf.Infinity;
        lockOnCamera.transform.position = mainCamera.transform.position;
        playerCamera.transform.rotation = mainCamera.transform.rotation;

        for (int i = 0; i < enemies.Length; i++)
        {
            //distance between Player and Enemy;
            var diff = (enemies[i].transform.position - position);
            var curDistance = diff.magnitude;
            
            // check if an Enemy is close enough
            if (curDistance < 100f)
            {
                //if an Enemy is close enough, check if its in sight of the cam
                enemyPos = mainCamera.WorldToViewportPoint(enemies[i].transform.position);
                if (enemyPos.x >= 0 && enemyPos.y >= 0 && enemyPos.z >= 0 && enemyPos.x <= 1 && enemyPos.y <= 1 && !TargetObstructed(enemies[i]))
                {
                    //check how far the Enemy is away from the center of the screen   
                    enemyPos -= new Vector3(0.5f, 0.5f, 0);
                    enemyPos.z = 0;
                    var curEnemyPos = enemyPos.magnitude;
                    if (curEnemyPos < dot)
                    {
                        dot = curEnemyPos;
                        closestEnemy = enemies[i];                      
                    }
                    else if (curEnemyPos > dot)
                    {
                        continue;
                    }
                    enemyPos = new Vector3(0, 0, 0);
                }

                //Enemy is not in CamSight  ---- turn Cam behind PLayer ----- 
                else
                {                    
                    continue;
                }
            }
        }

        targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = playerWeight };
        targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = closestEnemy.transform, radius = 4.0f, weight = 1.0f };
        ActivateLockOncam();
    }

    void ChangeTarget()
    {
        var position = player.transform.position;
        var dot = Mathf.Infinity;

        for (int i = 0; i < enemies.Length; i++)
        {
            //distance between Player and Enemy;        
            var diff = (enemies[i].transform.position - position);
            var curDistance = diff.magnitude;

            // check if an Enemy is close enough
            if (curDistance < 50f)
            {
                //if an Enemy is close enough, check if its in sight of the cam
                enemyPos = mainCamera.WorldToViewportPoint(enemies[i].transform.position);
                var closestEnemyPos = mainCamera.WorldToViewportPoint(closestEnemy.transform.position);

                if (enemyPos.x >= 0 && enemyPos.y >= 0 && enemyPos.z >= 0 && enemyPos.x <= 1 && enemyPos.y <= 1 && !TargetObstructed(closestEnemy))
                {

                    if (changeTarget < 0)
                    {
                        //change to left Target
                        if (enemyPos.x < closestEnemyPos.x)
                        {
                            //ignore depth values
                            enemyPos.z = 0f;
                            closestEnemyPos.z = 0f;
                            //take the closest to the current trageted enemy
                            var enemyDiff = (enemyPos - closestEnemyPos);
                            var curEnemyPos = enemyDiff.magnitude;

                            if (curEnemyPos < dot)
                            {
                                dot = curEnemyPos;
                                closestChangeEnemy = enemies[i];
                            }
                        }
                    }
                    else if (changeTarget > 0)
                    {
                        //change to right Target
                        if (enemyPos.x > closestEnemyPos.x)
                        {
                            //ignore depth values
                            enemyPos.z = 0f;
                            closestEnemyPos.z = 0f;
                            //take the closest to the current trageted enemy
                            var enemyDiff = (enemyPos - closestEnemyPos);
                            var curEnemyPos = enemyDiff.magnitude;

                            if (curEnemyPos < dot)
                            {
                                dot = curEnemyPos;
                                closestChangeEnemy = enemies[i];
                            }
                        }
                    }
                }
            }
        }
        closestEnemy = closestChangeEnemy;
        targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = playerWeight };
        targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = closestEnemy.transform, radius = 4.0f, weight = 1.0f };
    }

    void TargetIndicator() //Gets called in Update by invokeRepeating
    {
        //TargetIndicator
        if (closestEnemy != null)
        {
            targetIndicator.transform.position = mainCamera.WorldToScreenPoint(closestEnemy.transform.position);
        }
    }

    bool TargetObstructed(GameObject target)
    {
        Physics.Raycast(mainCamera.transform.position, target.transform.position - mainCamera.transform.position, out enemyRayHit,Mathf.Infinity , enemyRayLayerMask);
        if (enemyRayHit.collider.CompareTag("Enemy"))
        {
            //cam See's Enemy
            return false;                  
        }
        else
        {
            //Cam cant see Enemy
            return true;
        }
    }

    bool PlayerObstructed()
    {
        Physics.Raycast(mainCamera.transform.position, (player.transform.position + Vector3.up) - mainCamera.transform.position, out playerRayHit, Mathf.Infinity, playerRayLayerMask);
        if (playerRayHit.collider.CompareTag("Player"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void ActivateLockOncam()
    {      
        lockOnCam.Priority = 1;
        playerCam.Priority = 0;
    }

    void ActivatePlayerCam()
    {
        playerCam.Priority = 1;
        lockOnCam.Priority = 0;
    }

    void LookAtEnemy()
    {
        if (closestEnemy != null)
        {
            player.LookAt(new Vector3(closestEnemy.transform.position.x, player.position.y, closestEnemy.transform.position.z));
        }
    }
}
