using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFox : MonoBehaviour
{
    public delegate void AttackAction();
    public static event AttackAction HitPlayer;
    
    public Transform fuchs;
    public Transform character;
    public Transform dashGoal;
    public NavMeshAgent nmafuchs;    
    public Animator anim;

    private BoxCollider Attackhitbox;
    private float distanceToPlayer;
    public float aggroRange;
    public float attackRange;
    private bool isAttacking;
    [SerializeField]private Vector3 attackGoal;

    [SerializeField] private LayerMask playerLayerMask;

    public float stunDuration = .5f;
    public float health;

    private bool isDead = false;
    private PullEnemy pullEnemy;

    void Start()
    {
        Attackhitbox = GetComponentsInChildren<BoxCollider>()[1];
        Attackhitbox.enabled = false;
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
        if (distanceToPlayer <= aggroRange && PlayerIsVisible() && !isAttacking)
        {
            NmaSetTarget();  
        }
        else if (distanceToPlayer >= aggroRange && PlayerIsVisible() || isAttacking)
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
                StopCoroutine(AttackMove());
                StartCoroutine(AttackMove());
                attackGoal = dashGoal.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HitPlayer();
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
        NmaRemoveTarget();
        anim.SetTrigger("prepareAttack"); 
        yield return new WaitForSeconds(.5f);
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(.5f);
        Attackhitbox.enabled = true;
        while (transform.position != attackGoal)
        {
            transform.position = Vector3.MoveTowards(transform.position,attackGoal, .6f);
            yield return null;
        }
        yield return new WaitForSeconds(.5f);
        Attackhitbox.enabled = false;
        yield return new WaitForSeconds(1f);
        NmaSetTarget();
        isAttacking = false;
    }
}
