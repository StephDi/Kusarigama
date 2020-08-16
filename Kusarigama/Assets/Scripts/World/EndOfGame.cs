using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGame : MonoBehaviour
{

    [SerializeField] private GameObject SecretLaterneFull;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
        yield return new WaitForSeconds(5);
        EndOfGameMenu();

    } 
}


