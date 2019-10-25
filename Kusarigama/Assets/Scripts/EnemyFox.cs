using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFox : MonoBehaviour
{
    public Transform fuchs;
    public Transform character;
    public NavMeshAgent nmafuchs;    
    public Animator anim;

    public float Health;

    void Start()
    {       
        Health = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (NavMeshAgentManager.instance.chasing == true)
        {
            nmafuchs.SetDestination(character.position);
            nmafuchs.nextPosition = fuchs.transform.position;
            nmafuchs.updatePosition = true;
            anim.SetBool("moving", true);

        }
        else if (NavMeshAgentManager.instance.chasing == false)
        {
            nmafuchs.ResetPath();
            nmafuchs.updatePosition = false;
            anim.SetBool("moving", false);
        }
    }

    //Take damage if a hit is detected -> MeleeCombat
    public void TakeDamage()
    {
        Health = Health - Gamemanager.instance.Damage;
        anim.SetTrigger("damageTaken");

        if (Health <= 0)
        {
            EnemyDie();
        }
    }

    void EnemyDie()
    {
        if (!anim.GetBool("isDead"))
        {
            anim.SetBool("isDead", true);
        }
        nmafuchs.speed = 0f;
        Destroy(this.gameObject,3f);
    }
}
