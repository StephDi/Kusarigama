using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    public Transform player;
    //Cameras
    public GameObject playerCamera;
    public GameObject lockOnCamera;
    public Camera mainCamera;
    //Enemies
    public GameObject[] enemies;
    public GameObject closestEnemy;  
    //Raycast
    public bool enemyObstructed;
    //Target
    public Cinemachine.CinemachineTargetGroup group;
    public Cinemachine.CinemachineTargetGroup.Target[] targets;
    public Transform targetIndicator;
    public Vector3 enemyPos;
    public float changeTarget;
    public bool changedTarget;
    //Timer
    public float waitForUnLock;
    public bool waited;


    private void Start()
    {
        targets = group.m_Targets;
        waitForUnLock = 0.3f;
        changedTarget = false;
    }

    void Update()
    {
        changeTarget = Input.GetAxis("Mouse X");


        //Press the Button to Lock on 
        if (Input.GetButtonDown("LockOn") && lockOnCamera.activeSelf == false)
        {
            FindClosestEnemy();
            targetIndicator.gameObject.SetActive(true);
            //set the Targetindicator every 0.002s 
            InvokeRepeating("TargetIndicator",0,0.002f);
        }
        else if (Input.GetButtonDown("LockOn") && lockOnCamera.activeSelf == true || closestEnemy == null || enemyObstructed == true)
        {
            ActivatePlayerCam();
            targetIndicator.gameObject.SetActive(false);
            closestEnemy = null;
        }

        //changeTarget
        if (changeTarget != 0 && lockOnCamera.activeSelf == true && changedTarget == false)
        {
            changedTarget = true;
            ChangeTarget();
        }
        if (changeTarget == 0)
        {
            changedTarget = false;
        }

        if (closestEnemy == null)
        {
            targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
            targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = null, radius = 4.0f, weight = 1.0f };
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
                    ActivatePlayerCam();
                    targetIndicator.gameObject.SetActive(false);
                }
            }
        }
    }

    void TargetIndicator()
    {
        //TargetIndicator
        if (closestEnemy != null)
        {
            targetIndicator.transform.position = mainCamera.WorldToScreenPoint(closestEnemy.transform.position);
        }
    }

    void FindClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var position = player.transform.position;
        var dot = Mathf.Infinity;
        lockOnCamera.transform.position = mainCamera.transform.position;
        playerCamera.transform.rotation = mainCamera.transform.rotation;

        foreach (GameObject e in enemies)
        {
            //distance between Player and Enemy;
            var diff = (e.transform.position - position);
            var curDistance = diff.sqrMagnitude;
            
            // check if an Enemy is close enough
            if (curDistance < 1500f)
            {
                //if an Enemy is close enough, check if its in sight of the cam
                enemyPos = mainCamera.WorldToViewportPoint(e.transform.position);
                if (enemyPos.x >= 0 && enemyPos.y >= 0 && enemyPos.z >= 0 && enemyPos.x <= 1 && enemyPos.y <= 1)
                {
                    //check how far the Enemy is away from the center of the screen   
                    enemyPos -= new Vector3(0.5f, 0.5f, 0);
                    enemyPos.z = 0;
                    var curEnemyPos = enemyPos.magnitude;
                    if (curEnemyPos < dot)
                    {
                        dot = curEnemyPos;
                        closestEnemy = e;
                        targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
                        targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = closestEnemy.transform, radius = 4.0f, weight = 1.0f };
                        ActivateLockOncam();
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
                    //targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
                    //targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = null, radius = 4.0f, weight = 1.0f };
                    //closestEnemy = null;
                    //ActivatePlayerCam();
                    continue;
                }
            }
        }
    }

    void ChangeTarget()
    {
        var position = player.transform.position;
        var dot = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            //distance between Player and Enemy;
            var diff = (e.transform.position - position);
            var curDistance = diff.sqrMagnitude;

            // check if an Enemy is close enough
            if (curDistance < 1500f)
            {
                //if an Enemy is close enough, check if its in sight of the cam
                enemyPos = mainCamera.WorldToViewportPoint(e.transform.position);
                var closestEnemyPos = mainCamera.WorldToViewportPoint(closestEnemy.transform.position);

                if (enemyPos.x >= 0 && enemyPos.y >= 0 && enemyPos.z >= 0 && enemyPos.x <= 1 && enemyPos.y <= 1)
                {

                    if (changeTarget < 0)
                    {
                        //change to left Target
                        if (enemyPos.x < closestEnemyPos.x)
                        {
                            //take the closest to the current trageted enemy
                            var enemyDiff = (closestEnemyPos - enemyPos);
                            var curEnemyPos = enemyDiff.sqrMagnitude;

                            if (curEnemyPos < dot)
                            {
                                dot = curEnemyPos;
                                closestEnemy = e;
                                targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
                                targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = closestEnemy.transform, radius = 4.0f, weight = 1.0f };
                                return;
                            }
                            closestEnemyPos = new Vector3(0, 0, 0);
                        }
                    }
                    else if (changeTarget > 0)
                    {
                        //change to right Target
                        if (enemyPos.x > closestEnemyPos.x)
                        {
                            //take the closest to the current trageted enemy
                            var enemyDiff = (closestEnemyPos - enemyPos);
                            var curEnemyPos = enemyDiff.sqrMagnitude;

                            if (curEnemyPos < dot)
                            {
                                dot = curEnemyPos;
                                closestEnemy = e;
                                targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
                                targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = closestEnemy.transform, radius = 4.0f, weight = 1.0f };
                                return;
                            }
                            closestEnemyPos = new Vector3(0, 0, 0);
                        }
                    }
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
