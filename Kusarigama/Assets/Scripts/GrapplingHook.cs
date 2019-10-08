using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public Transform Weapon;
    public Transform character;
    public GameObject HookedObject;

    bool hookEnemy = false;
    bool hookAnchor = false;

    void Update()
    {
        if (hookEnemy)
        {
            PullEnemy();
        }

        if (hookAnchor)
        {
            PullToAnchor();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //PullEnemy   
            HookedObject = other.gameObject;
            hookEnemy = true;
        }

        if (other.tag == "Anchor")
        {
            //PullToAnchor    
            HookedObject = other.gameObject;
            hookAnchor = true;
        }
    }

    void PullEnemy()
    {
        Debug.Log("enemy");
        HookedObject.transform.position = Vector3.Lerp(HookedObject.transform.position,character.position,0.1f);
        if (HookedObject.transform.position == character.position)
        {
            hookEnemy = false;
        }
    }

    void PullToAnchor()
    {
        Debug.Log("Anchor");
        character.position = Vector3.Lerp(character.position,HookedObject.transform.position,0.1f);
        if (character.position == HookedObject.transform.position)
        {
            hookAnchor = false;
        }
    }
}
