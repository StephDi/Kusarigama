using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStoneCollision : MonoBehaviour
{

    public bool Stone1;
    public bool Stone2;
    public bool Stone3;
    public bool Stone4;

    public Animator anim;

    void Start()
    {
        Stone1 = false;
        Stone2 = false;
        Stone3 = false;
        Stone4 = false;
    }

    void Update()
    {
        if (Stone1 && Stone2 && Stone3 && Stone4)
        {
            anim.SetBool("openDoor",true);
            FindObjectOfType<AudioManager>().Play("GateOpen");
        }    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RiddleStone")
        {
            Debug.Log("Hit" + this.gameObject);
            if (other.gameObject.name == "Rätselstein 1")
            {
                Stone1 = true;
            }
            if (other.gameObject.name == "Rätselstein 2")
            {
                Stone2 = true;
            }
            if (other.gameObject.name == "Rätselstein 3")
            {
                Stone3 = true;
            }
            if (other.gameObject.name == "Rätselstein 4")
            {
                Stone4 = true;
            }
        }
    }
}
