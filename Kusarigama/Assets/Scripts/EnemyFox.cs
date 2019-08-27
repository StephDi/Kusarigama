using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;

public class EnemyFox : MonoBehaviour {

    public int health;
    public Transform character;
    public NavMeshAgent fuchs;
    public Rigidbody rbFuchs;
    public Animator anim;

	// Use this for initialization
	void Start ()
    {
        health = 30;
        rbFuchs = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        fuchs.SetDestination(character.position);

        if (health <= 0)
        {           
            Destroy(gameObject);
        }

        if (rbFuchs.velocity.sqrMagnitude != 0)
        {
            anim.SetBool("moving",true);
        }
        else
        {
            anim.SetBool("moving",false);
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
