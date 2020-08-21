using UnityEngine;

public class TriggerDialogueOnCollision : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;

    private GetWeaponUpgrades getWeaponUpgrades;
    private bool gotTriggered;
    private void Awake()
    {
        getWeaponUpgrades = FindObjectOfType<GetWeaponUpgrades>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
        gotTriggered = false;
    }  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && getWeaponUpgrades.weaponState == WeaponState.NONE && gotTriggered == false)
        {
            dialogueTrigger.TriggerDialogue();
            gotTriggered = true;
        }        
    }
}
