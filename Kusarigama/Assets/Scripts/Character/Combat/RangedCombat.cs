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
    [SerializeField] private float rangedCombatRange;
    //Rotation
    public Transform RangedCombatRotation;
    public Transform RangedCombatRotationGoal;
    bool rotate = false;

    void Update()
    {
        rangedAttackTrigger = Input.GetAxis("Throw");

        if (rangedAttackTrigger > 0 && !Input.GetButton("Fire2"))
        {
            anim.SetTrigger("rangedAttack");
            FindObjectOfType<AudioManager>().Play("ThrowChain");
        }

        if (rotate == true)
        {
            RangedCombatRotation.localRotation = Quaternion.Slerp(RangedCombatRotation.localRotation, RangedCombatRotationGoal.localRotation, 0.15f);
        }
    }

    public void BeginArcRanged()
    {
        Weapon.SetParent(RangedCombatRotation);
        RangedCombatRotation.localRotation = Quaternion.Euler(0, 45f, 0);
        RangedCombatRotationGoal.localRotation = Quaternion.Euler(0, -45f, 0);
        Weapon.localPosition = new Vector3(0, 1f, rangedCombatRange);
        Weapon.rotation = RangedCombatRotation.rotation * Quaternion.Euler(0, 0, -90f);
        rotate = true;
    }

    public void EndArcRanged()
    {
        rotate = false;
        Weapon.SetParent(WeaponPoint);
        RangedCombatRotation.localRotation = Quaternion.Euler(0, 0, 0);
        Weapon.localRotation = Quaternion.identity;
        Weapon.localPosition = new Vector3(0, 0, 0.0028f);
    }

    public void BeginArcReverse()
    {
        Weapon.SetParent(RangedCombatRotation);
        RangedCombatRotation.localRotation = Quaternion.Euler(0, -45f, 0);
        RangedCombatRotationGoal.localRotation = Quaternion.Euler(0, 45f, 0);
        Weapon.localPosition = new Vector3(0, 1f, rangedCombatRange);
        Weapon.rotation = RangedCombatRotation.rotation * Quaternion.Euler(0, 0, 90f);
        rotate = true;
    }


    //Event
    public void ActivateWeaponColliderRanged()
    {
        weaponCollider.enabled = true;
    }

    //Event
    public void DeActivateWeaponColliderRanged()
    {
        weaponCollider.enabled = false;
    }
}
