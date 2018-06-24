using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public static int damage;
    public int weaponDamage;
    public BoxCollider weaponCollider;
    public float attackCooldown;

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

            //Set Weapontrigger = true
            weaponCollider.enabled = true;
            StartCoroutine(Attack());
        }
		
	}

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        weaponCollider.enabled = false;
    }
}
