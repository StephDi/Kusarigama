using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallReset : MonoBehaviour
{
    [SerializeField] private float maxFallDepth;

    private Vector3 spawnPoint;

    private void Update()
    {
        spawnPoint = new Vector3(transform.position.x,Terrain.activeTerrain.SampleHeight(transform.position) + 5f,transform.position.z);
        maxFallDepth = Terrain.activeTerrain.SampleHeight(transform.position)-25;

        if (transform.position.y <= maxFallDepth)
        {
            transform.position = spawnPoint;
        }
    }
}
