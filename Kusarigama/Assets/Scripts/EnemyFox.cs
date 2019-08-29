using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFox : MonoBehaviour
{
    public Transform fuchs;
    public Transform character;
    public NavMeshAgent nmafuchs;
    public Rigidbody rbFuchs;
    public Animator anim;

    void Start()
    {
        rbFuchs = fuchs.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NavMeshAgentManager.instance.chasing == true)
        {
            nmafuchs.SetDestination(character.position);
            nmafuchs.updatePosition = true;

        }
        else if (NavMeshAgentManager.instance.chasing == false)
        {
            nmafuchs.ResetPath();
            nmafuchs.updatePosition = false;
        } 

        if (rbFuchs.velocity.sqrMagnitude > 0)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }
}
