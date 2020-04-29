using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSakura : MonoBehaviour
{
    public delegate void HealPlayerAction();
    public static event HealPlayerAction HealPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<CharacterHealth>().health <= 90)
            {
                HealPlayer();
                Destroy(gameObject);
            }
        }
    }
}
