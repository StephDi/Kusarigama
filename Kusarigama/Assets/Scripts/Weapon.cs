using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public static int damage;
    public int weaponDamage;
    public BoxCollider weaponCollider;
    public float attackCooldown; 
    public Animator anim;

	// Use this for initialization
	void Start () {
		
	}

    void Update() {
        damage = weaponDamage;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        //Simple Attack
        if (Input.GetButtonDown("Fire1"))
        {
            //Play Animation
            anim.SetBool("Attack",true);
            //Set Weapontrigger = true
            weaponCollider.enabled = true;
            StartCoroutine(Attack());
        }
		
	}

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        weaponCollider.enabled = false;
        anim.SetBool("Attack",false);
    }
}
