using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public static float health;

    void Start()
    {
        health = 100f;    
    }

    void Update()
    {
        if (health <= 0f)
        {
            CharacterDie();
        }    
    }

    void CharacterDie()
    {

    }
}
