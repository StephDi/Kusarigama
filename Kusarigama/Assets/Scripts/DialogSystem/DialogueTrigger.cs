using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]private UiType uiType;
    public Dialogue dialogue;
    private enum UiType{DIALOGUE, TUTORIAL};
    public void TriggerDialogue()
    {
        switch(uiType)
        {
            case UiType.DIALOGUE:
            FindObjectOfType<DialogueManager>().StartDialog(dialogue);
                break;
            case UiType.TUTORIAL:
            FindObjectOfType<DialogueManager>().StartTutorial(dialogue);
                break;
            default:
                break;

        }
    }
}
