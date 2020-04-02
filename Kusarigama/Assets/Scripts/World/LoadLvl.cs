using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLvl : MonoBehaviour
{
    public int LvlIndexToLoad;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Scenemanager.instance.LoadLevel(LvlIndexToLoad);
        }    
    }
}
