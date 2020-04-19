using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public float health;
    [SerializeField] private Image HealthbarImage;
    [SerializeField] private Animator anim;
    [SerializeField] private CapsuleCollider playerCollider;

    private CharMovement charMovement;

    void OnEnable()
    {
        EnemyFox.HitPlayer += GetDamage;
    }

    void Start()
    {
        charMovement = GetComponent<CharMovement>();
        playerCollider = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        health = 100f;    
    }

    void OnDisable()
    {
        EnemyFox.HitPlayer -= GetDamage;
    }

    void GetDamage()
    {
        if (health >= 1f)
        {
            health -= 10f;
            HealthbarImage.fillAmount = health / 100f;
            anim.SetTrigger("damageTaken");         
            if(health <= 0f)
            {
                CharacterDie();
            }
        }
    }

    void CharacterDie()
    {
        anim.SetBool("isDead",true);
        playerCollider.direction = 2;
        charMovement.enabled = false;
    }
}
