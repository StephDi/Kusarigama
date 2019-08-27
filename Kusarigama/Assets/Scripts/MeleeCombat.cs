using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour {

    public BoxCollider weaponCollider;
    public float attackCooldown; 
    public Animator anim;
    public CharMovement charMovement;

    void Start()
    {
        charMovement = GetComponent<CharMovement>();   
    }

    // Update is called once per frame
    void FixedUpdate () {

        //Simple Attack
        if (Input.GetButtonDown("Fire1"))
        {
            //Play Animation
            anim.SetTrigger("attack");
            if(charMovement.movement.sqrMagnitude != 0)
            {
                anim.SetLayerWeight(1, 1f);
            }
            
            //Set Weapontrigger = true
            weaponCollider.enabled = true;
            StartCoroutine(Attack());
        }
		
	}

    public void ActivateWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DeActivateWeaponCollider()
    {
        weaponCollider.enabled = false;
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        weaponCollider.enabled = false;
        anim.SetLayerWeight(1,0f);
    }
}
