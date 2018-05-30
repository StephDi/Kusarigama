using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponatChar : MonoBehaviour {

    public GameObject character;
    public GameObject weaponAnchor;
	
	// Update is called once per frame
	void FixedUpdate () {

        weaponAnchor.transform.position = character.transform.position;


	}
}
