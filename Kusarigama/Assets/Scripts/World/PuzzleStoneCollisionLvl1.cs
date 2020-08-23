using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStoneCollisionLvl1 : MonoBehaviour
{
    public bool Stone1;
    public bool Stone2;

    public Animator anim;

    [SerializeField] private Color emissionColor;
    private PuzzleStoneCollisionLvl1Ghost puzzleStoneCollisionLvl1Ghost;
    private PuzzleStoneCollisionLvl1Ranged puzzleStoneCollisionLvl1Ranged;
    [SerializeField]private ThrowWeapon throwWeapon;

    void Start()
    {
        throwWeapon = FindObjectOfType<ThrowWeapon>();
        puzzleStoneCollisionLvl1Ghost = FindObjectOfType<PuzzleStoneCollisionLvl1Ghost>();
        Stone1 = false;
        Stone2 = false;
    }

    private void Update()
    {
        if (throwWeapon.hit.collider != null)
        {
            if (throwWeapon.hit.collider.name == "RätselsteinLvl1 3")
            {
                if (!Stone2)
                {
                    Stone2 = true;
                    AudioManager.instance.Play("PuzzlestoneHit3");
                    throwWeapon.hit.collider.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
                }
                if ((Stone1 || puzzleStoneCollisionLvl1Ranged.Stone1) && Stone2 && puzzleStoneCollisionLvl1Ghost.StoneGhost && !anim.GetBool("openDoor"))
                {
                    OpenDoor();
                }
            }

        }
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
                if (!Stone1 && !puzzleStoneCollisionLvl1Ranged.Stone1)
                {
                    Stone1 = true;
                    puzzleStoneCollisionLvl1Ranged.Stone1 = true;
                    AudioManager.instance.Play("PuzzlestoneHit1");
                    other.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
                }
            }

            if (other.gameObject.name == "RätselsteinLvl1 3")
            {
                if (!Stone2)
                {
                    Stone2 = true;
                    AudioManager.instance.Play("PuzzlestoneHit3");
                    other.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
                }
            }           

            if ((Stone1 || puzzleStoneCollisionLvl1Ranged.Stone1) && Stone2 && puzzleStoneCollisionLvl1Ghost.StoneGhost && !anim.GetBool("openDoor"))
            {
                OpenDoor();
            }
        }
    }
}
