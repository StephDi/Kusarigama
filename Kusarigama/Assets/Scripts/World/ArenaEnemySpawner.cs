using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject firstFight;
    [SerializeField] private GameObject secondFight;
    [SerializeField] private GameObject thirdFight;

    private void OnEnable()
    {
        EnemyFox.FoxDies += TestForNextWave;
    }

    private void OnDisable()
    {
        EnemyFox.FoxDies -= TestForNextWave;
    }

    private void Update()
    {
        TestForNextWave();
    }

    void TestForNextWave()
    {
        if (firstFight.transform.childCount == 1)
        {
            secondFight.SetActive(true);
        }
        if (secondFight.transform.childCount == 1)
        {
            thirdFight.SetActive(true);
        }
    }
}
