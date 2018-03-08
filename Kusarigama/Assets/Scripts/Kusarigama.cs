using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kusarigama : MonoBehaviour {

    public Transform weaponTarget;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = weaponTarget.position;
    }
}
