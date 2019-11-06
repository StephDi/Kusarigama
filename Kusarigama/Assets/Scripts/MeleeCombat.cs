using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour {

    public BoxCollider weaponCollider;
    public Animator anim;
    public CharMovement charMovement;
    public RangedCombatGhost rangedCombat;

    void Start()
    {
        charMovement = GetComponent<CharMovement>();
        rangedCombat = GetComponent<RangedCombatGhost>();
    }

    // Update is called once per frame
    void Update() {

        //Simple Attack Melee right
        if (Input.GetButtonDown("Fire1") && !Input.GetButton("Fire2"))
        {
            //set mecanim State
            anim.SetTrigger("attack");

            if (charMovement.movement.sqrMagnitude != 0)
            {
                anim.SetLayerWeight(1, 1f);
            }
            else if (charMovement.movement.sqrMagnitude == 0)
            {
                anim.SetLayerWeight(1, 0f);
            }
        }

        if (charMovement.movement.sqrMagnitude == 0 || AnimationIsPlaying(anim,"Idle") || rangedCombat.rangedAttackTriggerGhost < 0)
            {
                anim.SetLayerWeight(1, 0f);
            }
    }

    //Check if the Animation "stateMame" is PLaying
    bool AnimationIsPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(1).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }

    //Event
    public void ResetTrigger()
    {
        anim.ResetTrigger("attack");
        anim.ResetTrigger("rangedAttack");
    }

    //Event
    public void ActivateWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    //Event
    public void DeActivateWeaponCollider()
    {
        weaponCollider.enabled = false;
    }

}
