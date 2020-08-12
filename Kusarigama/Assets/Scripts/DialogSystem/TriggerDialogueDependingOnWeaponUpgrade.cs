using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueDependingOnWeaponUpgrade : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;
    private GetWeaponUpgrades getWeaponUpgrades;
    private PuzzleStoneCollisionLvl1 puzzleStoneCollisionLvl1;
    private PuzzleStoneCollisionLvl1Ghost puzzleStoneCollisionLvl1Ghost;
    private bool gotTriggered = false;

    private void Awake()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        getWeaponUpgrades = FindObjectOfType<GetWeaponUpgrades>();
        puzzleStoneCollisionLvl1 = FindObjectOfType<PuzzleStoneCollisionLvl1>();
        puzzleStoneCollisionLvl1Ghost = FindObjectOfType<PuzzleStoneCollisionLvl1Ghost>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gotTriggered)
        {
            if (other.CompareTag("Player") && getWeaponUpgrades.weaponState == WeaponState.MELEECHAIN && !puzzleStoneCollisionLvl1.Stone2)
            {
                dialogueTrigger.dialogue.sentences[0] = "Now I can reach the upper stone.";
                dialogueTrigger.TriggerDialogue();
            }
            if (other.CompareTag("Player") && getWeaponUpgrades.weaponState == WeaponState.MELEEGHOST && !puzzleStoneCollisionLvl1Ghost.StoneGhost)
            {
                dialogueTrigger.dialogue.sentences[0] = "Now I can power the blue stone.";
                dialogueTrigger.TriggerDialogue();
            }
            if (other.CompareTag("Player") && getWeaponUpgrades.weaponState == WeaponState.MELEECHAINGHOST && (!puzzleStoneCollisionLvl1.Stone2 || !puzzleStoneCollisionLvl1Ghost.StoneGhost))
            {
                dialogueTrigger.dialogue.sentences[0] = "Now I can reach the upper stone and power the blue stone!";
                dialogueTrigger.TriggerDialogue();
                if (puzzleStoneCollisionLvl1.Stone2 || puzzleStoneCollisionLvl1Ghost.StoneGhost)
                {
                    gotTriggered = true;
                }
            }
        }
        else if (gotTriggered)
        {
            Destroy(this.gameObject);
        }
    }
}
