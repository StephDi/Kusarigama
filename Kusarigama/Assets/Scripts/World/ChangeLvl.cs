using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLvl : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Gamemanager.instance.Level1Done = true;
            Scenemanager.instance.changeLvl = true;
        }  
    }
}
