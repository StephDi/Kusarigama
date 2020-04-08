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

    private float distanceToPlayer;
    public float aggroRange;
    public float attackRange;

    [SerializeField] private LayerMask playerLayerMask;

    public float stunDuration = .5f;
    public float health;

    public bool isDead = false;
    private PullEnemy pullEnemy;

    private bool isAttacking;


    void Start()
    {
        pullEnemy = FindObjectOfType<PullEnemy>();
        health = 30;
        NmaRemoveTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.instance.panel.activeInHierarchy) 
        {
            GetDistanceToPlayer();
            AggroPlayer();
            EnemyAttack();
        }
        else
        {
            NmaRemoveTarget();
        }
    }

    void AggroPlayer()
    {
        if (distanceToPlayer <= aggroRange && PlayerIsVisible())
        {
            NmaSetTarget();  
        }
        else if (distanceToPlayer >= aggroRange && PlayerIsVisible())
        {
            NmaRemoveTarget();
        }
    }

    void GetDistanceToPlayer()
    {
        distanceToPlayer = Vector3.Distance(this.transform.position,character.transform.position);
    }

    bool PlayerIsVisible()
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position, character.transform.position - this.transform.position, out hit, Mathf.Infinity, playerLayerMask);
        return hit.collider.CompareTag("Player");
    }

    public void NmaSetTarget()
    {

        nmafuchs.SetDestination(character.position);
        nmafuchs.nextPosition = fuchs.transform.position;
        nmafuchs.updatePosition = true;
        anim.SetBool("moving", true);

    }

    public void NmaRemoveTarget()
    {

        nmafuchs.ResetPath();
        nmafuchs.updatePosition = false;
        anim.SetBool("moving", false);

    }

    //Take damage if a hit is detected -> MeleeCombat
    public void TakeDamage()
    {
        if (!isDead)
        {
            health -= Playermanager.instance.Damage;
            anim.SetTrigger("damageTaken");
            FindObjectOfType<AudioManager>().Play("FoxHurt");
            nmafuchs.speed = 0;
            StartCoroutine(DamageTakenSlow());
        }
        if (health <= 0)
        {
            EnemyDie();
            isDead = true;
            nmafuchs.speed = 0f;
            FindObjectOfType<AudioManager>().Play("FoxDying");

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
    

    void EnemyAttack()
    {
        if (distanceToPlayer >= attackRange)
            return;

        if (distanceToPlayer <= attackRange)
        {
           
            if (!isAttacking)
            {
                StartCoroutine(AttackMove());
            }
        }
    }

    IEnumerator DamageTakenSlow()
    {
        yield return new WaitForSeconds(stunDuration);
        if (!isDead && this.gameObject != pullEnemy.hookedObject)
        {
            nmafuchs.speed = 4f;
        }
    }

    IEnumerator AttackMove()
    {
        isAttacking = true;
        yield return new WaitForSeconds(.5f);
        anim.SetTrigger("attack");
        anim.SetBool("attackbool", true);
        yield return new WaitForSeconds(5f);
        isAttacking = false;
    }
}
