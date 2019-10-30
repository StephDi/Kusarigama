using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombat : MonoBehaviour
{
    //Input
    public float rangedAttackTrigger;
    //Animation
    public Animator anim;
    //Weapon
    public Transform Weapon;
    public Collider weaponCollider;
    public Transform WeaponPoint;

    void Update()
    {
        rangedAttackTrigger = Input.GetAxis("Throw");

        if (rangedAttackTrigger > 0 && !Input.GetButton("Fire2"))
        {
            anim.SetTrigger("rangedAttack");
        }
    }

    //Event
    public void ActivateWeaponColliderRanged()
    {
        weaponCollider.enabled = true;
    }

    //Event
    public void DeActivateWeaponColliderGhost()
    {
        weaponCollider.enabled = false;
    }
}
