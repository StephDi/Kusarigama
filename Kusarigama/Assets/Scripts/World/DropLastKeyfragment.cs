using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLastKeyfragment : MonoBehaviour
{
    [SerializeField] private GameObject Keyfragment;

    private EnemyBear enemyBear;
    private Vector3 lastEnemyPosition;
    private bool spawned;

    private void Awake()
    {
        spawned = false;
        enemyBear = GetComponent<EnemyBear>();
        Keyfragment.SetActive(false);
    }

    private void Update()
    {
        if (enemyBear.health <= 0 && spawned == false)
        {
            SpawnKeyfragment();
        }
    }

    void SpawnKeyfragment()
    {
        spawned = true;
        lastEnemyPosition = transform.position;
        Keyfragment.transform.position = lastEnemyPosition;
        StartCoroutine(SpawnafterTime());
    }

    IEnumerator SpawnafterTime()
    {
        yield return new WaitForSeconds(2.5f);
        Keyfragment.SetActive(true);
    }
}
