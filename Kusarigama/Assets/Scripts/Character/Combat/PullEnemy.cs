using UnityEngine;
using System.Collections;

public class PullEnemy : MonoBehaviour
{
    public Transform character;
    public Rigidbody rbCharacter;
    public BoxCollider weaponCollider;
    public GameObject hookedObject = null;
    private float pullBackTime = 3f;

    public bool hookEnemy = false;
    public bool hanging = false;

    public Animator anim;

    private ThrowWeapon throwWeapon;
    void Start()
    {
        throwWeapon = FindObjectOfType<ThrowWeapon>();
        weaponCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {     
        if (hookedObject != null)
        {
            if (anim.GetBool("pullBack"))
            {
                PullEnemyToPlayer();
            }
            else
            {
                EnemyHit();
            }
        }
    }

    public void EnemyHit()
    {
        weaponCollider.enabled = false;
        if (hookedObject.TryGetComponent(out EnemyFox enemyFox)) 
        {
            enemyFox.nmafuchs.speed = 0f;
        }
        if (hookedObject.TryGetComponent(out EnemyBear enemyBear)) 
        {
            enemyBear.nmabear.speed = 0f;
        }
        hookEnemy = true;
        transform.parent.position = hookedObject.transform.position; 
        if (!throwWeapon.throwingInput)
        {
            StopCoroutine("PullbackTimer");
            PullEnemyToPlayer();
        }
        else
        {
            StartCoroutine(PullbackTimer());
        }
    }

    void PullEnemyToPlayer()
    {
        if (hookedObject != null)
        {
            hookedObject.transform.position = Vector3.Lerp(hookedObject.transform.position, character.position + new Vector3(0,1,0), 0.1f);
            float dist = Vector3.Distance(hookedObject.transform.position, character.position);
            if (dist < 4f)
            {
                hookEnemy = false;
                hookedObject = null;
            }
            StartCoroutine(SetHookedObjectToNullAfterTime());
        }
    }

    public IEnumerator PullbackTimer()
    {
        yield return new WaitForSeconds(pullBackTime);
        if (hookedObject != null)
        {
            if (hookedObject.TryGetComponent(out EnemyFox enemyFox))
            {
                enemyFox.nmafuchs.speed = 4f;
            }
            if (hookedObject.TryGetComponent(out EnemyBear enemyBear))
            {
                enemyBear.nmabear.speed = 4f;
            }
            anim.SetBool("pullBack",true);
            PullEnemyToPlayer();
        }
    }

    IEnumerator SetHookedObjectToNullAfterTime()
    {
        yield return new WaitForSeconds(1.5f);
        hookedObject = null;
    }
}
