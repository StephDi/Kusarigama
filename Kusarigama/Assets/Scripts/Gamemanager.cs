using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour {

    public static Gamemanager instance = null;
    public static int currentDamage;
    public int currentDamage1;

    void Awake()
    {
       
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    void Update() {
        currentDamage = Weapon.damage;
        currentDamage1 = currentDamage;
    }

}
