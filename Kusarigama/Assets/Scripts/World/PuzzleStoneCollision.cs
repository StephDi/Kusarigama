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

    [SerializeField] private Color emissionColor;

    void Start()
    {
        Stone1 = false;
        Stone2 = false;
        Stone3 = false;
        Stone4 = false;
    }

    void OpenDoor()
    {
        anim.SetBool("openDoor", true);
        AudioManager.instance.Play("GateOpen");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RiddleStone")
        {
            if (other.gameObject.name == "Rätselstein 1")
            {
                if (!Stone1)
                {
                    Stone1 = true;
                    AudioManager.instance.Play("PuzzlestoneHit1");
                    other.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
                }
            }

            if (other.gameObject.name == "Rätselstein 2")
            {
                if (!Stone2)
                {
                    Stone2 = true;
                    AudioManager.instance.Play("PuzzlestoneHit2");
                    other.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
                }
            }

            if (other.gameObject.name == "Rätselstein 3")
            {

                if (!Stone3)
                {
                    Stone3 = true;
                    AudioManager.instance.Play("PuzzlestoneHit3");
                    other.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
                }
            }

            if (other.gameObject.name == "Rätselstein 4")
            {
                if (!Stone4)
                {
                    Stone4 = true;
                    AudioManager.instance.Play("PuzzlestoneHit4");
                    other.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
                }
            }

            if (Stone1 && Stone2 && Stone3 && Stone4 && !anim.GetBool("openDoor"))
            {
                OpenDoor();
            }
        }
    }
}
