using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float moveSpeed = 100f;
    public Rigidbody weaponAnchor;

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("WeaponHorizontal");
        float vertical = Input.GetAxis("WeaponVertical");

        Vector3 movement = new Vector3(horizontal * moveSpeed * Time.deltaTime, 0, vertical * moveSpeed * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            weaponAnchor.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F));
        }

    }
}

