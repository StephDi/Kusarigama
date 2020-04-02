using UnityEngine.Audio;
using UnityEngine;

public class Sound_Footstep1 : MonoBehaviour
{


    // Update is called once per frame
    public void Footstep1()
    {
        FindObjectOfType<AudioManager>().Play("Footstep1");
    }
}
