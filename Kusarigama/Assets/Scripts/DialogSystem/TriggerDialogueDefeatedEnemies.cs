using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueDefeatedEnemies : MonoBehaviour
{
    [SerializeField] private List<EnemyFox> foxesToKill;

    private DialogueTrigger dialogueTrigger;
    [SerializeField] private int foxesKilled = 0;
    private int foxesToKillArrayStartLength;

    private void Awake()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        foxesToKillArrayStartLength = foxesToKill.Count;
    }

    private void OnEnable()
    {
        EnemyFox.FoxDies += TestForDialogueTrigger;
    }

    private void OnDisable()
    {
        EnemyFox.FoxDies -= TestForDialogueTrigger;
    }

    void TestForDialogueTrigger()
    {
        if (foxesToKill.Count == 0)
        {
            return;
        }
        for (int i = 0; i < foxesToKill.Count; i++)
        {
            if (foxesToKill[i].isDead)
            {
                foxesToKill.RemoveAt(i);
                foxesKilled++;
            }
        }

        if (foxesKilled >= foxesToKillArrayStartLength)
        {
            dialogueTrigger.TriggerDialogue();
        }
    }
}
