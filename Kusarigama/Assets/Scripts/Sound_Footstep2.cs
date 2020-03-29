using UnityEngine.Audio;
using UnityEngine;

public class Sound_Footstep2 : MonoBehaviour
{


    // Update is called once per frame
    public void Footstep2()
    {
        FindObjectOfType<AudioManager>().Play("Footstep2");
    }
}
