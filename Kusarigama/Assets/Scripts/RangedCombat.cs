using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombat : MonoBehaviour
{
    //Input
    public float rangedAttackTrigger;
    //Animation
    public Animator anim;
    //Timer
    public float cooldown;
    bool canAttackAOE = true;
    //Weapon
    public Transform GhostWeapon;
    public Collider weaponCollider;

    void Update()
    {

        rangedAttackTrigger = Input.GetAxis("AoeAttack");

        if (rangedAttackTrigger > 0 && !Input.GetButton("Fire2"))
        {
            if (canAttackAOE == true)
            {
                anim.SetTrigger("rangedAttack");
                canAttackAOE = false;
                StartCoroutine(AOECooldown());
            }
        }
    }

    public void BeginArc()
    {
        GhostWeapon.transform.localPosition = new Vector3(0,-0.05f,0.048f);
    }

    public void EndArc()
    {
        GhostWeapon.transform.localPosition = new Vector3(0,0,0.0028f);
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
