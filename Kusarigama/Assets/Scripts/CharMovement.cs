﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour {

    public float moveSpeed = 10f;
    public Rigidbody character;
    public float dashForce;
    public bool dashCooldown = true;
    public float dashDuration;
    public float jumpForce;

	// Use this for initialization
	void Start () {
        character = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal * moveSpeed * Time.deltaTime, 0, vertical * moveSpeed * Time.deltaTime);

        character.MovePosition(transform.position + movement);
        if (movement != Vector3.zero)
        {
            character.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.35F));
        }

        //Dash
        if ((Input.GetButtonDown("Fire3")) && (dashCooldown == true))
        {
            Debug.Log("X");
            // moveSpeed = 30f;
            character.AddForce(movement.normalized * dashForce);
            dashCooldown = false;
            StartCoroutine(Dash());
        }

        //jump
        if (Input.GetButtonDown("Fire1") )
        {
            character.AddForce(0,jumpForce,0);
        }
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashDuration);
        //moveSpeed = 10f;
        dashCooldown = true;
    }
}
