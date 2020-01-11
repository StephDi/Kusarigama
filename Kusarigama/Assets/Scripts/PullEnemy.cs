using UnityEngine;

public class PullEnemy : MonoBehaviour
{
    public Transform weapon;
    public Transform character;
    public Rigidbody rbCharacter;
    public BoxCollider weaponCollider;
    private GameObject HookedObject;

    public bool hookEnemy = false;
    public bool hanging = false;

    public Animator anim;

    void Start()
    {
        weapon = GetComponent<Transform>();
        weaponCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (hookEnemy)
        {
            transform.parent.position = HookedObject.transform.position;

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
            HookedObject = other.gameObject;
            hookEnemy = true;      
        }
        else
        {
            hookEnemy = false;
        }
    }

    void PullEnemyToPlayer()
    {
        if (HookedObject != null)
        {
            HookedObject.transform.position = Vector3.Lerp(HookedObject.transform.position, character.position + new Vector3(0,1,0), 0.1f);
            float dist = Vector3.Distance(HookedObject.transform.position, character.position);
            if (dist < 4f)
            {
                hookEnemy = false;
            }
        }
    }
}
