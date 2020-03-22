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
    [SerializeField] private LayerMask playerLayerMask;

    public float stunDuration = .5f;
    public float Health;

    public bool isDead = false;
    private PullEnemy pullEnemy;


    void Start()
    {
        pullEnemy = FindObjectOfType<PullEnemy>();
        Health = 30;
        NmaRemoveTarget();
    }

    // Update is called once per frame
    void Update()
    {
        GetDistanceToPlayer();
        AggroPlayer();
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
        if (NavMeshAgentManager.instance.chasing == true)
        {
            nmafuchs.SetDestination(character.position);
            nmafuchs.nextPosition = fuchs.transform.position;
            nmafuchs.updatePosition = true;
            anim.SetBool("moving", true);

        }
    }

    public void NmaRemoveTarget()
    {
        if (NavMeshAgentManager.instance.chasing == false)
        {
            nmafuchs.ResetPath();
            nmafuchs.updatePosition = false;
            anim.SetBool("moving", false);
        }
    }

    //Take damage if a hit is detected -> MeleeCombat
    public void TakeDamage()
    {
        if (!isDead)
        {
            Health = Health - Gamemanager.instance.Damage;
            anim.SetTrigger("damageTaken");
            nmafuchs.speed = 0;
            StartCoroutine(DamageTakenSlow());
        }
        if (Health <= 0)
        {
            EnemyDie();
            isDead = true;
            nmafuchs.speed = 0f;
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

    IEnumerator DamageTakenSlow()
    {
        yield return new WaitForSeconds(stunDuration);
        if (!isDead && this.gameObject != pullEnemy.hookedObject)
        {
            nmafuchs.speed = 4f;
        }
    }
}
