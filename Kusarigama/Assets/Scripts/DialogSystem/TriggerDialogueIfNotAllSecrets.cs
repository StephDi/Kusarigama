using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueIfNotAllSecrets : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;
    private CollectKeyFragment collectKeyFragment;

    private void Awake()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        collectKeyFragment = FindObjectOfType<CollectKeyFragment>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !collectKeyFragment.ContainsKey(KeyFragment.KeyType.L1) | !collectKeyFragment.ContainsKey(KeyFragment.KeyType.R2))
        {
            dialogueTrigger.TriggerDialogue();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
