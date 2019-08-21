using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{

    public AudioClip footStep;
    public AudioSource audioS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Footstep()
    {
        audioS.PlayOneShot(footStep);
    }
}
