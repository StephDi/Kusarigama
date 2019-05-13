using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{

    public Transform enemy;
    public Transform targetIndicator;
    // Start is called before the first frame update
    void Start()
    {
        targetIndicator = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LockOn")) 
            // Check if Enemy is in range and in sight of Cam
        {
            targetIndicator.position = enemy.position + new Vector3(0, 1, 0);
        }
    }


}
