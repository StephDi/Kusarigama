using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFox : MonoBehaviour {

    public int health;

	// Use this for initialization
	void Start ()
    {
        health = 30;
	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0) {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Weapon") {
            health = health - Gamemanager.currentDamage;
        }
    }

    //EnemyMovement

}
