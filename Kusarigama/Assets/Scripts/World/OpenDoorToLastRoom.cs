using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoorToLastRoom : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToKill;

    private DialogueTrigger dialogueTrigger;
    private int enemiesKilled = 0;
    private int foxesToKillArrayStartLength;

    private CollectKeyFragment collectKeyFragment;
    private bool gotTriggered;

    [SerializeField]private Animator anim;

    private void Awake()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        collectKeyFragment = FindObjectOfType<CollectKeyFragment>();
        foxesToKillArrayStartLength = enemiesToKill.Count;   
        gotTriggered = false;
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
    }

    private void OnTriggerEnter(Collider other)
    {       
        if (enemiesKilled >= foxesToKillArrayStartLength)      
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

                        anim.SetBool("openDoor", true);
                        gotTriggered = true;
                    }
                    else
                    {
                        dialogueTrigger.TriggerDialogue();
                    }
                }
            }
        }
    }
}
