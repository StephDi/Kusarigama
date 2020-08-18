﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBear : MonoBehaviour
{
    public delegate void FoxAttackAction();
    public static event FoxAttackAction HitPlayer;

    public delegate void BearDiesEvent();
    public static event BearDiesEvent BearDies;
    
    public Transform bear;
    public Transform character;
    public Transform dashGoal;
    public NavMeshAgent nmabear;    
    public Animator anim;
    public bool isDead = false;

    public float aggroRange;
    public float aggroRangeshort;
    public float attackRange;
    public float stunDuration = .5f;
    public float health;

    [SerializeField]private Vector3 attackGoal;
    [SerializeField] private LayerMask playerLayerMask;

    private BoxCollider Attackhitbox;
    private float distanceToPlayer;
    private bool isAttacking;
    private PullEnemy pullEnemy;
    private FoxHealth foxHealth;

    void Start()
    {
        character = GameObject.Find("Character").transform;
        foxHealth = GetComponentInChildren<FoxHealth>();
        Attackhitbox = GetComponentsInChildren<BoxCollider>()[1];
        Attackhitbox.enabled = false;
        pullEnemy = FindObjectOfType<PullEnemy>();
        health = 100;
        NmaRemoveTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.instance.panel != null)
        {
            if (UIManager.instance.panel.activeInHierarchy | DialogueManager.instance.DialogueIsActive)
            {
                NmaRemoveTarget();
            }
            else
            {
                GetDistanceToPlayer();
                AggroPlayer();
                EnemyAttack();
            }
        }

    }

    void AggroPlayer()
    {
        if (distanceToPlayer <= aggroRange && PlayerIsVisible() && PlayerIsInFront() && !isAttacking && !isDead)
        {
            NmaSetTarget();
            foxHealth.UpdateUI(health);
        }
        else if (distanceToPlayer <= aggroRangeshort && !isAttacking && !isDead)
        {
            NmaSetTarget();
            foxHealth.UpdateUI(health);
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
        if (hit.collider != null)
        {
            return hit.collider.CompareTag("Player");
        }
        else
        {
            return false;
        }
    }

    bool PlayerIsInFront()
    {

        Vector3 dirToPlayer = character.position - transform.position;
        float angleBetweenEnemyAndCharacter = Vector3.Angle(dirToPlayer, transform.forward);
        if(angleBetweenEnemyAndCharacter <= 60f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool PlayerIsInAttackCone()
    {

        Vector3 dirToPlayer = character.position - transform.position;
        float angleBetweenEnemyAndCharacter = Vector3.Angle(dirToPlayer, transform.forward);
        if (angleBetweenEnemyAndCharacter <= 25f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void NmaSetTarget()
    {
        nmabear.SetDestination(character.position);
        nmabear.nextPosition = bear.transform.position;
        nmabear.updatePosition = true;
        anim.SetBool("moving", true);
    }

    public void NmaRemoveTarget()
    {
        nmabear.ResetPath();
        nmabear.updatePosition = false;
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
            FindObjectOfType<AudioManager>().Play("EnemyHit");
            nmabear.speed = 0;
            foxHealth.UpdateUI(health);
            StartCoroutine(DamageTakenSlow());
        }
        if (health <= 0 && !isDead)
        {
            isDead = true;
            nmabear.speed = 0f;
            FindObjectOfType<AudioManager>().Play("FoxDying");
            EnemyDie();

        }
    }

    void EnemyDie()
    {
        if (!anim.GetBool("isDead"))
        {
            anim.SetBool("isDead", true);
            
        }
        BearDies();
        NmaRemoveTarget();
        nmabear.speed = 0f;
        Destroy(this.gameObject,3f);
    }
    

    void EnemyAttack()
    {
        if (distanceToPlayer >= attackRange)
            return;

        if (distanceToPlayer <= attackRange && PlayerIsInAttackCone() && !isDead)
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
            nmabear.speed = 4f;
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
        AudioManager.instance.Play("FoxAttack");
        while (Vector3.Distance(transform.position,attackGoal) >= .2f)
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