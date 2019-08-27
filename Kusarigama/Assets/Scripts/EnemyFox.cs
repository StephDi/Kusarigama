using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFox : MonoBehaviour {

    public int health;
    public Transform character;
    public NavMeshAgent fuchs;
	// Use this for initialization
	void Start ()
    {
        health = 30;
	}
	
	// Update is called once per frame
	void Update ()
    {
        fuchs.SetDestination(character.position);
        if (health <= 0)
        {
            
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Weapon")
        {
      
        }
    }

    //EnemyMovement


}
