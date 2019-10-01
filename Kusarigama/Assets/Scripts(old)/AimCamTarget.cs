using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCamTarget : MonoBehaviour
{
    public Transform AimingTargetTarget;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = AimingTargetTarget.position;
    }
}
