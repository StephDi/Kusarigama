using UnityEngine.Audio;
using UnityEngine;

public class Sound_Footsteps3 : MonoBehaviour
{


    // Update is called once per frame
    public void Footstep3()
    {
        FindObjectOfType<AudioManager>().Play("Footstep3");
    }
}
