using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerTutorialOnTriggerEnter : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;
    private bool gotTriggered;
    private void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        gotTriggered = false;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gotTriggered == false)
        {
            dialogueTrigger.TriggerDialogue();
            gotTriggered = true;
        }
    }
}
