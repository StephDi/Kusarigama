using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public Transform weapon;
    public Transform character;
    public Rigidbody rbCharacter;
    public BoxCollider weaponCollider;
    private GameObject HookedObject;

    public bool hookEnemy = false;
    public bool hookAnchor = false;

    float grappleSpeed = 25f;
    float forwardspeed = 5f;
    float upForce = 5f;

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
                PullEnemy();
            }
        }

        if (hookAnchor)
        {
            if (anim.GetBool("pullBack") == true)
            {
                PullToAnchor();
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

        if (other.tag == "Anchor" && anim.GetBool("throwing") == true)
        {
            //PullToAnchor    
            weaponCollider.enabled = false;
            HookedObject = other.gameObject;
            hookAnchor = true;

            transform.parent.position = other.transform.position;
        }
        else
        {
            hookAnchor = false;
        }
    }

    void PullEnemy()
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

    void PullToAnchor()
    {
        character.position = Vector3.Lerp(character.position, HookedObject.transform.localPosition, 0.1f);

        float dist = Vector3.Distance(character.position, HookedObject.transform.localPosition + new Vector3 (0,5,3));
        float distance = (HookedObject.transform.position - character.transform.position).magnitude;
        Vector3 direction = HookedObject.transform.position - character.transform.position;
        if (distance > 2f)
        {
            //rbCharacter.AddForce((direction * grappleSpeed) + (Vector3.forward * forwardspeed) + (Vector3.up * upForce));
        }
        if (distance < 3f)
        {
            hookAnchor = false;
        }
    }
}
