using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;

public class NavMeshAgentManager : MonoBehaviour {

    public static NavMeshAgentManager instance = null;

    public float chaseDuration;
    public bool chasing;

    void Awake()
    {
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        chasing = true;
        StartCoroutine(ChaseCooldown());
    }

    IEnumerator ChaseCooldown()
    {
        yield return new WaitForSeconds(chaseDuration);
        chasing = !chasing;
        StartCoroutine(ChaseCooldown());
    }
}
