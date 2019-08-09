using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine.Editor;

public class EnemyLockOn : MonoBehaviour
{
    public Transform player;
    //Cameras
    public GameObject playerCamera;
    public GameObject lockOnCamera;
    public Camera mainCamera;
    //Enemies
    public GameObject[] enemies;
    List<GameObject> closeEnemies = new List<GameObject>();
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
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        targets = group.m_Targets;
        waitForUnLock = 0.3f;
        changedTarget = false;
    }

    void Update()
    {
        changeTarget = Input.GetAxis("Mouse X");


        //Press the Button to Lock on 
        if (Input.GetButtonDown("LockOn") && playerCamera.activeSelf == true)
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
        var position = player.transform.position;
        var dot = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            //distance between Player and Enemy;
            var diff = (e.transform.position - position);
            var curDistance = diff.sqrMagnitude;
            
            // check if an Enemy is close enough
            if (curDistance < 1000f)
            {
                //if an Enemy is close enough, check if its in sight of the cam
                enemyPos = mainCamera.WorldToViewportPoint(e.transform.position);

                if (enemyPos.x >= 0 && enemyPos.y >= 0 && enemyPos.z >= 0 && enemyPos.x <= 1 && enemyPos.y <= 1)
                {
                    //check how far the Enemy is away from the center of the screen    
                    enemyPos -= new Vector3(0.5f, 0.5f, 0);
                    enemyPos.z = 0;
                    var curEnemyPos = enemyPos.sqrMagnitude;

                    if (curEnemyPos < dot)
                    {
                        dot = curEnemyPos;
                        closestEnemy = e;
                        targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
                        targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = closestEnemy.transform, radius = 4.0f, weight = 1.0f };
                        ActivateLockOncam();                       
                    }                 
                    enemyPos = new Vector3(0, 0, 0);
                }

                //Enemy is not in CamSight  ---- turn Cam behind PLayer ----- 
                else
                {
                    targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
                    targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = null, radius = 4.0f, weight = 1.0f };
                    closestEnemy = null;
                    ActivatePlayerCam();
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
            if (curDistance < 1000f)
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
                            var enemyDiff = (closestEnemyPos -enemyPos);
                            var curEnemyPos = enemyDiff.sqrMagnitude;

                            if (curEnemyPos < dot)
                            {
                                Debug.Log(closestEnemy);
                                dot = curEnemyPos;
                                closestEnemy = e;
                                targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
                                targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = closestEnemy.transform, radius = 4.0f, weight = 1.0f };
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
                                Debug.Log(closestEnemy);
                                dot = curEnemyPos;
                                closestEnemy = e;
                                targets[0] = new Cinemachine.CinemachineTargetGroup.Target { target = player, radius = 4.0f, weight = 1.0f };
                                targets[1] = new Cinemachine.CinemachineTargetGroup.Target { target = closestEnemy.transform, radius = 4.0f, weight = 1.0f };
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
