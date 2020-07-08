using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueIfMissingSecrets : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;
    private CollectKeyFragment collectKeyFragment;
    private bool gotTriggered;

    private void Awake()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        collectKeyFragment = FindObjectOfType<CollectKeyFragment>();
        gotTriggered = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!gotTriggered)
        {
            if (collision.collider.CompareTag("Player") && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L1) && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.R2))
            {
                dialogueTrigger.dialogue.sentences[0] = "Alright. I’m still missing 2 key fragments.";
                dialogueTrigger.dialogue.sentences[1] = "I know there should be one near the village and one near the stone circle.";
                dialogueTrigger.TriggerDialogue();
            }
            if (collision.collider.CompareTag("Player") && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L1) && collectKeyFragment.ContainsKey(KeyFragment.KeyType.R2))
            {
                dialogueTrigger.dialogue.sentences[0] = "Alright. I’m still missing 1 key fragments.";
                dialogueTrigger.dialogue.sentences[1] = "I know there should be one near the village and one near the stone circle.";
                dialogueTrigger.TriggerDialogue();
            }
            if (collision.collider.CompareTag("Player") && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L1) && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.R2))
            {
                dialogueTrigger.dialogue.sentences[0] = "Alright. I’m still missing 1 key fragments.";
                dialogueTrigger.dialogue.sentences[1] = "I know there should be one near the village and one near the stone circle.";
                dialogueTrigger.TriggerDialogue();
            }
            if (collision.collider.CompareTag("Player") && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L1) && collectKeyFragment.ContainsKey(KeyFragment.KeyType.R2))
            {
                dialogueTrigger.dialogue.sentences[0] = "Great. Since I have the first two of the key fragments, I can go through.";
                dialogueTrigger.TriggerDialogue();
                gotTriggered = true;
            }
        }
        else if(gotTriggered)
        {
            Destroy(this.gameObject);
        }
    }
}
