﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform WeaponPoint;

    private void Awake()
    {
        ResetWeapon();
    }

    public void ResetWeapon()
    {
        transform.gameObject.SetActive(true);
        transform.SetParent(WeaponPoint);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
