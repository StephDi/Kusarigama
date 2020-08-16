using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorToLastRoom : MonoBehaviour
{
    //private DialogueTrigger dialogueTrigger;
    private CollectKeyFragment collectKeyFragment;
    private bool gotTriggered;

    [SerializeField]private Animator anim;

    private void Awake()
    {
        //dialogueTrigger = GetComponent<DialogueTrigger>();
        collectKeyFragment = FindObjectOfType<CollectKeyFragment>();
        gotTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!gotTriggered)
            {
                if (other.CompareTag("Player") && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L2) 
                                               && collectKeyFragment.ContainsKey(KeyFragment.KeyType.L3) 
                                               && collectKeyFragment.ContainsKey(KeyFragment.KeyType.R1) 
                                               && collectKeyFragment.ContainsKey(KeyFragment.KeyType.R3))
                {
                    //dialogueTrigger.dialogue.sentences.RemoveAt(1);
                    //dialogueTrigger.dialogue.sentences[0] = "Done";
                    //dialogueTrigger.TriggerDialogue();
                    anim.SetBool("openDoor",true);
                    gotTriggered = true;
                }
            }
            else if (gotTriggered)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
