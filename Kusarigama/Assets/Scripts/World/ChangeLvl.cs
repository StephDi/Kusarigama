using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLvl : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Scenemanager.instance.changeLvl = true;
        }  
    }
}
