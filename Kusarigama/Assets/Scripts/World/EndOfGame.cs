using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGame : MonoBehaviour
{

    [SerializeField] private GameObject SecretLaterneFull;

    private bool gottriggered = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !gottriggered)
        {
            gottriggered = true;
            SecretLaterneFull.SetActive(true);
            StartCoroutine(WaitForEndOfGame());
        }
    }

    void EndOfGameMenu()
    {
        UIManager.instance.OpenEndMenu();
    }

   IEnumerator WaitForEndOfGame()
    {
        AudioManager.instance.Play("WaterSplash");
        AudioManager.instance.Play("EndMusic");
        yield return new WaitForSeconds(5);
        Gamemanager.instance.Level1Done = false;
        UIManager.instance.gameStarted = false;
        EndOfGameMenu();

    } 
}


