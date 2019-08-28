using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;

public class NavMeshAgentManager : MonoBehaviour {

    public Transform fuchs;
    public Transform character;
    public NavMeshAgent nmafuchs;
    public Rigidbody rbFuchs;
    public Animator anim;
    public float chaseDuration;

	// Use this for initialization
	void Start ()
    {    
        rbFuchs = fuchs.GetComponent<Rigidbody>();
        StartCoroutine(ChaseCooldown());      
        nmafuchs.updatePosition = false;
        
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (nmafuchs.updatePosition == true)
        {
            nmafuchs.SetDestination(character.position);
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

    IEnumerator ChaseCooldown()
    {
        yield return new WaitForSeconds(chaseDuration);
        Debug.Log("5sec");
        if (nmafuchs.updatePosition == false)
        {
            Debug.Log("destset");
            nmafuchs.updatePosition = true;
        }

        else if (nmafuchs.updatePosition == true)
        {
            Debug.Log("null");
            nmafuchs.updatePosition = false;
            nmafuchs.ResetPath();
        }

        StartCoroutine(ChaseCooldown());
    }
}
