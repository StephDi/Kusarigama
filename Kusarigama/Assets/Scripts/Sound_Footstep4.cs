using UnityEngine.Audio;
using UnityEngine;

public class Sound_Footstep4 : MonoBehaviour
{


    // Update is called once per frame
    public void Footstep4()
    {
        FindObjectOfType<AudioManager>().Play("Footstep4");
    }
}
