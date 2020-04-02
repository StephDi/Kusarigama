using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform Player;

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.position + Vector3.up * 1.5f;
    }
}
