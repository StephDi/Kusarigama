using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAtChar : MonoBehaviour {

    public Transform Char;
    public Transform Weapon;
	
	// Update is called once per frame
	void Update () {
        Weapon.position = Char.position;
	}
}
