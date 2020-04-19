using UnityEngine.Audio;
using UnityEngine;

public class Sound_Footstep : MonoBehaviour
{
    [SerializeField]private AudioClip[] footStepSounds;
    [SerializeField] private float sourceVolume = .5f;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = sourceVolume;
    }

    public void Footstep()
    {      
        audioSource.clip = footStepSounds[Random.Range(0,footStepSounds.Length)];
        audioSource.Play();
    }
}
