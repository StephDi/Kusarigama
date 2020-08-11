using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public float health;
    [SerializeField] private Image HealthbarImage;
    [SerializeField] private Animator anim;
    [SerializeField] private CapsuleCollider playerCollider;

    private CharMovement charMovement;
    private MeleeCombat meleeCombat;
    private RangedCombat rangedCombat;
    private RangedCombatGhost rangedCombatGhost;
    private AimWeapon aimWeapon;
    private ThrowWeapon throwWeapon;

    void OnEnable()
    {
        EnemyFox.HitPlayer += GetDamage;
        HealthSakura.HealPlayer += GetHealth;
    }

    private void Awake()
    {
        charMovement = GetComponent<CharMovement>();
        meleeCombat = GetComponent<MeleeCombat>();
        rangedCombat = GetComponent<RangedCombat>();
        rangedCombatGhost = GetComponent<RangedCombatGhost>();
        aimWeapon = GetComponent<AimWeapon>();
        throwWeapon = GetComponent<ThrowWeapon>();
        playerCollider = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
            
    }

    void Start()
    {
        health = 100f;    
    }

    void OnDisable()
    {
        EnemyFox.HitPlayer -= GetDamage;
        HealthSakura.HealPlayer -= GetHealth;
    }

    void GetDamage()
    {
        if (health >= 1f)
        {
           
            health -= 10f;
            HealthbarImage.fillAmount = health / 100f;
            anim.SetTrigger("damageTaken");
            FindObjectOfType<AudioManager>().Play("CharacterHit");
            if (health <= 0f)
            {
                FindObjectOfType<AudioManager>().Play("CharacterDied");
                CharacterDie();
            }
        }
    }

    void GetHealth()
    {
        if (health <= 90f)
        {
            FindObjectOfType<AudioManager>().Play("SakuraConsumed");
            health += 10f;
            HealthbarImage.fillAmount = health / 100f;
        }
    }

    void CharacterDie()
    {
        anim.SetBool("isDead",true);
        playerCollider.direction = 2;
        charMovement.enabled = false;
        meleeCombat.enabled = false;
        rangedCombat.enabled = false;
        rangedCombatGhost.enabled = false;
        aimWeapon.enabled = false;
        throwWeapon.enabled = false;
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(5);
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeScene);
    }
}
