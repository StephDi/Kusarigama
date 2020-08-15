using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorialAtStart : MonoBehaviour
{

    private DialogueTrigger dialogueTrigger;
    // Start is called before the first frame update
    public void StartTutorial()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        dialogueTrigger.TriggerDialogue();
    }
}
