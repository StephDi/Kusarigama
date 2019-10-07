using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrapplingHook : MonoBehaviour
{
    private AimWeapon aimWeapon;

    public Transform aimingTarget;

    public GameObject[] grapplingHookObjectsEnemy;
    public GameObject[] grapplingHookObjectsAnchor;
    public GameObject[] grapplingHookObjectsAll;

    void Start()
    {
        aimWeapon = GetComponent<AimWeapon>();    
    }

    void Update()
    {
        if (aimWeapon.aiming == true)
        {
            FindClosestGrapplingHookObject();
        }    
    }

    void FindClosestGrapplingHookObject()
    {
        //Get an Array of all grappable Objects
        grapplingHookObjectsEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        grapplingHookObjectsAnchor = GameObject.FindGameObjectsWithTag("Anchor");

        if (grapplingHookObjectsEnemy != null && grapplingHookObjectsAnchor != null)
        {
            grapplingHookObjectsAll = grapplingHookObjectsEnemy.Concat(grapplingHookObjectsAnchor).ToArray();
        }
        else if (grapplingHookObjectsEnemy != null && grapplingHookObjectsAnchor == null)
        {
            grapplingHookObjectsAll.Equals(grapplingHookObjectsEnemy);
        }
        else if (grapplingHookObjectsEnemy == null && grapplingHookObjectsAnchor != null)
        {
            grapplingHookObjectsAll = grapplingHookObjectsAnchor;
        }
        //else if (grapplingHookObjectsEnemy == null && grapplingHookObjectsAnchor == null)
        //{
        //    grapplingHookObjectsAll = null;
        //}

        if (grapplingHookObjectsAll != null)
        {
            foreach (GameObject grappO in grapplingHookObjectsAll)
            {
                //FindClosestObject
            }
        }
    }
}
