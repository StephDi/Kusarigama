using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStoneCollisionLvl1Ranged : MonoBehaviour
{
    public bool Stone1;

    public Animator anim;

    [SerializeField] private Color emissionColor;
    private PuzzleStoneCollisionLvl1Ghost puzzleStoneCollisionLvl1Ghost;
    private PuzzleStoneCollisionLvl1 puzzleStoneCollisionLvl;

    void Start()
    {
        puzzleStoneCollisionLvl1Ghost = FindObjectOfType<PuzzleStoneCollisionLvl1Ghost>();
        puzzleStoneCollisionLvl = FindObjectOfType<PuzzleStoneCollisionLvl1>();
        Stone1 = false;      
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
            if (other.gameObject.name == "RätselsteinLvl1 1")
            {
                if (!Stone1 && !puzzleStoneCollisionLvl.Stone1)
                {
                    Stone1 = true;
                    puzzleStoneCollisionLvl.Stone1 = true;
                    AudioManager.instance.Play("PuzzlestoneHit1");
                    other.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
                }
            }         

            if ((Stone1 || puzzleStoneCollisionLvl.Stone1) && puzzleStoneCollisionLvl.Stone2 && puzzleStoneCollisionLvl1Ghost.StoneGhost && !anim.GetBool("openDoor"))
            {
                OpenDoor();
            }
        }
    }
}
