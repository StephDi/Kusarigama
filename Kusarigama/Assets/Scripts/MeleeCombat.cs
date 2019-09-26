using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour {

    public BoxCollider weaponCollider;
    public float attackCooldown;
    public Animator anim;
    public CharMovement charMovement;
    public int combatAnimLayer = 1;

    void Start()
    {
        charMovement = GetComponent<CharMovement>();
    }

    // Update is called once per frame
    void Update() {

        //Simple Attack
        if (Input.GetButtonDown("Fire1"))
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

    }

    //Check for EnemyHit
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //Damage the Enemy
            Destroy(other);
        }
    }

    //Event
    public void ResetTrigger()
    {
        anim.ResetTrigger("attack");
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
