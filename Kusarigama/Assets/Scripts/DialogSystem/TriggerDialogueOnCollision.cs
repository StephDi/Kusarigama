using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueOnCollision : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;

    private GetWeaponUpgrades getWeaponUpgrades;

    private void Awake()
    {
        getWeaponUpgrades = FindObjectOfType<GetWeaponUpgrades>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && getWeaponUpgrades.weaponState == WeaponState.NONE)
        {
            dialogueTrigger.TriggerDialogue();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
