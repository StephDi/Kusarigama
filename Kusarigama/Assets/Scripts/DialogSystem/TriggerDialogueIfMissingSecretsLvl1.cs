using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerDialogueIfMissingSecretsLvl1 : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        if (!gotTriggered)
        {
            if (other.CompareTag("Player") && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L2) 
                                           && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L3)
                                           && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.R1))
            {
                dialogueTrigger.dialogue.sentences[0] = "I’m still missing 3 key fragments from down here.";
                dialogueTrigger.dialogue.sentences[1] = "One is at the overlook on the right. Two are in canyons.";
                dialogueTrigger.TriggerDialogue();
            }
            if (other.CompareTag("Player") && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L2)
                                           && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L3)
                                           && collectKeyFragment.ContainsKey(KeyFragment.KeyType.R1))
            {
                dialogueTrigger.dialogue.sentences[0] = "I’m still missing 2 key fragments from down here.";
                dialogueTrigger.dialogue.sentences[1] = "One is at the overlook on the right. Two are in canyons.";
                dialogueTrigger.TriggerDialogue();
            }
            if (other.CompareTag("Player") && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L2)
                                           && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L3)
                                           && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.R1))
            {
                dialogueTrigger.dialogue.sentences[0] = "I’m still missing 2 key fragments from down here.";
                dialogueTrigger.dialogue.sentences[1] = "One is at the overlook on the right. Two are in canyons.";
                dialogueTrigger.TriggerDialogue();
            }
            if (other.CompareTag("Player") && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L2)
                                           && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L3)
                                           && collectKeyFragment.ContainsKey(KeyFragment.KeyType.R1))
            {
                dialogueTrigger.dialogue.sentences[0] = "I’m still missing 1 key fragments from down here.";
                dialogueTrigger.dialogue.sentences[1] = "One is at the overlook on the right. Two are in canyons.";
                dialogueTrigger.TriggerDialogue();
            }
            if (other.CompareTag("Player") && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L2)
                                           && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L3)
                                           && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.R1))
            {
                dialogueTrigger.dialogue.sentences[0] = "I’m still missing 2 key fragments from down here.";
                dialogueTrigger.dialogue.sentences[1] = "One is at the overlook on the right. Two are in canyons.";
                dialogueTrigger.TriggerDialogue();
            }
            if (other.CompareTag("Player") && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L2)
                                           && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L3)
                                           && collectKeyFragment.ContainsKey(KeyFragment.KeyType.R1))
            {
                dialogueTrigger.dialogue.sentences[0] = "I’m still missing 1 key fragments from down here.";
                dialogueTrigger.dialogue.sentences[1] = "One is at the overlook on the right. Two are in canyons.";
                dialogueTrigger.TriggerDialogue();
            }
            if (other.CompareTag("Player") && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L2)
                                           && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L3)
                                           && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.R1))
            {
                dialogueTrigger.dialogue.sentences[0] = "I’m still missing 1 key fragments from down here.";
                dialogueTrigger.dialogue.sentences[1] = "One is at the overlook on the right. Two are in canyons.";
                dialogueTrigger.TriggerDialogue();
            }
            

            if (other.CompareTag("Player") && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L2)
                                           && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L3)
                                           && collectKeyFragment.ContainsKey(KeyFragment.KeyType.R1))
            {
                dialogueTrigger.dialogue.sentences.RemoveAt(1);
                dialogueTrigger.dialogue.sentences[0] = "Now I have all the fragments down here. The last one should be in the fortress on the hill.";
                dialogueTrigger.TriggerDialogue();
                gotTriggered = true;
            }
        }
    } 
}
