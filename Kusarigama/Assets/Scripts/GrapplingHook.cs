using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public Transform weapon;
    public Transform character;
    public BoxCollider weaponCollider;
    private GameObject HookedObject;

    bool hookEnemy = false;
    bool hookAnchor = false;

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
        if (other.tag == "Enemy")
        {
            //PullEnemy   
            weaponCollider.enabled = false;
            HookedObject = other.gameObject;
            hookEnemy = true;
        }

        if (other.tag == "Anchor")
        {
            //PullToAnchor    
            weaponCollider.enabled = false;
            HookedObject = other.gameObject;
            hookAnchor = true;
        }
    }

    void PullEnemy()
    {
        HookedObject.transform.position = Vector3.Lerp(HookedObject.transform.position,character.position,0.1f);
        float dist = Vector3.Distance(HookedObject.transform.position,character.position);
        if (dist < 3f)
        {
            hookEnemy = false;
        }
    }

    void PullToAnchor()
    {
        character.position = Vector3.Lerp(character.position,HookedObject.transform.position,0.1f);
        float dist = Vector3.Distance(character.position,HookedObject.transform.position);
        if (dist < 3f)
        {
            hookAnchor = false;
        }
    }
}
