using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrappleState { NONE, PULLING, HIT, HANGING, LETGO }

public class PullToAnchor : MonoBehaviour
{
    public GrappleState state;

    public GameObject character;
    public Rigidbody characterRb;
    public Animator anim;
    public GameObject hangingPoint;

    public GameObject hookedObject;
    public GameObject hangingposition;
    public ThrowWeapon throwWeapon;

    public float throwValue;

    void Start()
    {
        throwWeapon = FindObjectOfType<ThrowWeapon>();

        state = GrappleState.NONE;
    }

    void Update()
    {
        throwValue = Input.GetAxis("Throw");

        if (state == GrappleState.NONE)
        {
            characterRb.isKinematic = false;
            return;
        }
        if (state == GrappleState.HIT)
        {
            Debug.Log("HitAnchor");         
            transform.parent.position = hookedObject.transform.position;

            if (hangingposition != null)
            {
                character.transform.position = hangingposition.transform.position + new Vector3(0f, -3.5f, 0f);
            }

            if (throwValue < 0.3f)
            {
                state = GrappleState.PULLING;
            }
        }
        if (state == GrappleState.PULLING)
        {
            characterRb.isKinematic = true;
            float dist = Vector3.Distance(hookedObject.transform.position, hangingPoint.transform.position);
            if (dist > 2f) 
            {
                character.transform.position = Vector3.Lerp(character.transform.position, hookedObject.transform.position + new Vector3(0f,-3.5f,0f), 0.15f);
            }
            else
            {
                state = GrappleState.HANGING;
            }
        }
        if (state == GrappleState.HANGING)
        {
            anim.SetBool("hanging", true);
            characterRb.isKinematic = true;
            hangingposition = hookedObject;
            character.transform.position = hangingposition.transform.position + new Vector3(0f, -3.5f, 0f);
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Dash"))
            {
                state = GrappleState.LETGO;
            }
        }
        if (state == GrappleState.LETGO)
        {
            anim.SetBool("hanging", false);
            characterRb.isKinematic = false;
            hookedObject = null;
            hangingposition = null;
            state = GrappleState.NONE;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Anchor")
        {
            state = GrappleState.HIT;
            hookedObject = other.gameObject;  
        }
    }
}
