using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombatCollision : MonoBehaviour
{
    private BoxCollider weaponCollider;

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
            if (other.TryGetComponent(out EnemyFox enemyFox))
            {
                enemyFox.TakeDamage();
            }
            if (other.TryGetComponent(out EnemyBear enemyBear))
            {
                enemyBear.TakeDamage();
            }

            //other.GetComponent<EnemyFox>().TakeDamage();
            //other.GetComponent<EnemyBear>().TakeDamage();
            weaponCollider.enabled = false;
        }
    }
}
