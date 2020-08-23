using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombatCollisionGhost : MonoBehaviour
{
    private BoxCollider weaponCollider;

    private bool isColliding;
    void Start()
    {
        isColliding = false;
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false;
    }

    //Check for EnemyHit
    void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;
        if (other.tag == "Enemy" && weaponCollider.enabled == true)
        {
            if (other.TryGetComponent(out EnemyFox enemyFox))
            {
                if (enemyFox.canGetHit)
                {
                    Debug.Log("hit");
                    enemyFox.canGetHit = false;
                    enemyFox.TakeDamage(Playermanager.instance.RangedDamageGhost);
                }
            }

            if (other.TryGetComponent(out EnemyBear enemyBear))
            {
                Debug.Log("hit");
                enemyBear.TakeDamage(Playermanager.instance.RangedDamageGhost);
            }
        }
        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForEndOfFrame();
        isColliding = false;
    }
}
