using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombatCollision : MonoBehaviour
{

    public BoxCollider weaponCollider;

    void Start()
    {
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false;
    }

    //Check for EnemyHit
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && weaponCollider.enabled == true)
        {
            //Damage the Enemy -> EnemyFox
            Debug.Log("hit");

            other.GetComponent<EnemyFox>().TakeDamage();
            weaponCollider.enabled = false;
        }
    }
}
