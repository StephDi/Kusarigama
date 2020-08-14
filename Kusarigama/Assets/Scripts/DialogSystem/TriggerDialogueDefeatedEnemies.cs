using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueDefeatedEnemies : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToKill;

    private DialogueTrigger dialogueTrigger;
    private int enemiesKilled = 0;
    private int foxesToKillArrayStartLength;

    private void Awake()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        foxesToKillArrayStartLength = enemiesToKill.Count;
    }

    private void OnEnable()
    {
        EnemyFox.FoxDies += TestForDialogueTrigger;
        EnemyBear.BearDies += TestForDialogueTrigger;
    }

    private void OnDisable()
    {
        EnemyFox.FoxDies -= TestForDialogueTrigger;
        EnemyBear.BearDies -= TestForDialogueTrigger;
    }

    void TestForDialogueTrigger()
    {
        if (enemiesToKill.Count == 0)
        {
            return;
        }
        for (int i = 0; i < enemiesToKill.Count; i++)
        {
            enemiesToKill[i].TryGetComponent(out EnemyFox enemyFox);
            enemiesToKill[i].TryGetComponent(out EnemyBear enemyBear);
            if (enemyFox != null) 
            {
                if (enemyFox.isDead) 
                {
                    enemiesToKill.RemoveAt(i);
                    enemiesKilled++;
                }
            }
            if (enemyBear != null)
            {
                if (enemyBear.isDead)
                {
                    enemiesToKill.RemoveAt(i);
                    enemiesKilled++;
                }
            }
        }

        if (enemiesKilled >= foxesToKillArrayStartLength)
        {
            dialogueTrigger.TriggerDialogue();
        }
    }
}
