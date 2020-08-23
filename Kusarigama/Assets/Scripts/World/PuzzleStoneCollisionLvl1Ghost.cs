using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStoneCollisionLvl1Ghost : MonoBehaviour
{
    public bool StoneGhost;

    public Animator anim;

    

    [SerializeField][ColorUsage(true,true)] private Color emissionColor;
    private PuzzleStoneCollisionLvl1 puzzleStoneCollisionLvl1;
    private PuzzleStoneCollisionLvl1Ranged puzzleStoneCollisionLvl1Ranged;

    void Start()
    {
        puzzleStoneCollisionLvl1 = FindObjectOfType<PuzzleStoneCollisionLvl1>();
        puzzleStoneCollisionLvl1Ranged = FindObjectOfType<PuzzleStoneCollisionLvl1Ranged>();
        StoneGhost = false;
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
            if (other.gameObject.name == "RätselsteinLvl1 2")
            {
                if (!StoneGhost)
                {
                    StoneGhost = true;
                    AudioManager.instance.Play("PuzzlestoneHit1");
                    other.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
                }
            }          

            if ((puzzleStoneCollisionLvl1.Stone1 || puzzleStoneCollisionLvl1Ranged.Stone1) && puzzleStoneCollisionLvl1.Stone2 && StoneGhost && !anim.GetBool("openDoor"))
            {
                OpenDoor();
            }
        }
    }
}
