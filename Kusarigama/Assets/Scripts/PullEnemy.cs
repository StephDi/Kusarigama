using UnityEngine;
using System.Collections;

public class PullEnemy : MonoBehaviour
{
    public Transform character;
    public Rigidbody rbCharacter;
    public BoxCollider weaponCollider;
    public GameObject hookedObject;
    private float pullBackTime = 3f;

    public bool hookEnemy = false;
    public bool hanging = false;

    public Animator anim;

    void Start()
    {
        weaponCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (hookEnemy)
        {
            transform.parent.position = hookedObject.transform.position;

            if (anim.GetBool("pullBack") == true)
            {            
                PullEnemyToPlayer();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && anim.GetBool("throwing") == true)
        {
            //PullEnemy   
            weaponCollider.enabled = false;
            hookedObject = other.gameObject;
            hookedObject.GetComponent<EnemyFox>().nmafuchs.speed = 0f;
            hookEnemy = true;
            StartCoroutine(PullbackTimer());
        }
        else
        {
            hookEnemy = false;
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
            }
        }
    }

    IEnumerator PullbackTimer()
    {
        yield return new WaitForSeconds(pullBackTime);
        PullEnemyToPlayer();
        hookedObject.GetComponent<EnemyFox>().nmafuchs.speed = 4f;
        anim.SetBool("pullBack",true);
    }
}
