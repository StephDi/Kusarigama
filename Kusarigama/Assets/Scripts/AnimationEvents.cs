using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{

    public AudioClip footStep;

    public AudioSource audioS;




    void Footstep()
    {
        audioS.PlayOneShot(footStep);
    }
}
