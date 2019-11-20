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
    public bool hanging = false;

    float grappleSpeed = 25f;
    float forwardspeed = 5f;
    float upForce = 5f;
    float distance;

    public Animator anim;

    void Start()
    {
        weapon = GetComponent<Transform>();
        weaponCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (HookedObject != null) 
        {
            distance = ((HookedObject.transform.localPosition + new Vector3(0, -3.5f, 0)) - character.transform.position).magnitude;
        }

        if (hookEnemy)
        {
            transform.parent.position = HookedObject.transform.position;

            if (anim.GetBool("pullBack") == true)
            {            
                PullEnemy();
            }
        }

        if (hookAnchor || hanging)
        {
            if (anim.GetBool("pullBack") == true)
            {
                if (distance > 3f) 
                {
                    PullToAnchor();
                }
            }
            if (distance < 3f && hanging == true)
            {
                HangOnAnchor();
                LetAnchorGo();
                hookAnchor = false;
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
            if (HookedObject != null) 
            {
                transform.parent.position = other.transform.position;
            }
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
        character.position = Vector3.Lerp(character.position, HookedObject.transform.localPosition + new Vector3(0,-3.5f,0), 0.1f);
        anim.SetBool("hanging",false);
        hookAnchor = false;
        hanging = true;
    }

    void HangOnAnchor()
    {
        character.position = HookedObject.transform.position + new Vector3(0, -3.5f, 0);
        anim.SetBool("hanging",true);
    }

    void LetAnchorGo()
    {
        if (Input.GetAxis("Throw") > 0)
        {
            anim.SetBool("hanging", false);
            hookAnchor = false;
        }
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Dash"))
        {
            anim.SetBool("hanging",false);
            hanging = false;
        }
        
    }
}
