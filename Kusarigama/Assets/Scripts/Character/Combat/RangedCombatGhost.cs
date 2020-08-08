using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombatGhost : MonoBehaviour
{
    //Input
    public float rangedAttackTriggerGhost;
    //Animation
    public Animator anim;
    //Timer
    public float cooldown;
    bool canAttackAOE = true;
    //Weapon
    public Transform GhostWeapon;
    public Collider weaponCollider;
    public Transform WeaponPointGhost;
    [SerializeField] private float rangedWeaponRangeGhost;
    //Rotation
    public Transform RangedCombatRotation;
    public Transform RangedCombatRotationGoal;
    bool rotate = false;

    void Update()
    {

        rangedAttackTriggerGhost = Input.GetAxis("AoeAttack");

        if (rangedAttackTriggerGhost > 0 && !Input.GetButton("Fire2"))
        {
            if (canAttackAOE == true)
            {
                anim.SetTrigger("rangedAttackGhost");
                canAttackAOE = false;
                StartCoroutine(AOECooldown());
            }
        }

        if (rotate == true)
        {
            RangedCombatRotation.localRotation = Quaternion.Slerp(RangedCombatRotation.localRotation,RangedCombatRotationGoal.localRotation,0.2f);
            GhostWeapon.rotation = RangedCombatRotation.rotation * Quaternion.Euler(0,0,-90f);
        }
    }

    public void BeginArc()
    {
        GhostWeapon.SetParent(RangedCombatRotation);
        RangedCombatRotation.localRotation = Quaternion.Euler(0,45f,0);
        RangedCombatRotationGoal.localRotation = Quaternion.Euler(0,-45f,0);
        GhostWeapon.localPosition = new Vector3(0,1f,rangedWeaponRangeGhost);
        rotate = true;
    }

    public void EndArc()
    {
        rotate = false;
        GhostWeapon.SetParent(WeaponPointGhost);
        RangedCombatRotation.localRotation = Quaternion.Euler(0,0,0);
        GhostWeapon.localRotation = Quaternion.identity;
        GhostWeapon.localPosition = new Vector3(0,0,0.0028f);
    }

    //Event
    public void ActivateWeaponColliderGhost()
    {
        weaponCollider.enabled = true;
    }

    //Event
    public void DeActivateWeaponColliderGhost()
    {
        weaponCollider.enabled = false;
    }

    IEnumerator AOECooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canAttackAOE = true;
    }
}
