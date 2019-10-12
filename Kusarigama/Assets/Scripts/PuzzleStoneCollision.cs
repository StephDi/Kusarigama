using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStoneCollision : MonoBehaviour
{
    GameObject openDoor;

    void Start()
    {
 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Debug.Log("Hit" + this.gameObject);
            
        }
    }
}
