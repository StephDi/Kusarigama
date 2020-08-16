using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCheat : MonoBehaviour
{
    private int NumberOfButtonPresses;
    private bool activated = false;

    private void Update()
    {
        if (Input.GetButtonDown("Cheat"))
        {
            NumberOfButtonPresses++;
            Debug.Log(NumberOfButtonPresses);
            CheckForCheatActivation();
            StopCoroutine(FastPressesTimer());
            StartCoroutine(FastPressesTimer());
        }
    }

    void CheckForCheatActivation()
    {
        if (NumberOfButtonPresses >= 5)
        {
            Debug.Log("Cheat");
            if (!activated)
            {
                ActivateCheat();
            }
            else
            {
                DeactivateCheat();
            }
            NumberOfButtonPresses = 0;
        }
    }

    void ActivateCheat()
    {
        activated = true;
        GetComponent<CharacterHealth>().health = 1000f;
        GetComponent<CharacterHealth>().maxHealth = 1000f;
    }

    void DeactivateCheat()
    {
        activated = false;
        GetComponent<CharacterHealth>().health = 100f;
        GetComponent<CharacterHealth>().maxHealth = 100f;
    }

    IEnumerator FastPressesTimer()
    {
        yield return new WaitForSeconds(1f);
        NumberOfButtonPresses = 0;
    }
}
