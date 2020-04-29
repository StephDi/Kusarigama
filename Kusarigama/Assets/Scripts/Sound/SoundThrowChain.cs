using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundThrowChain : MonoBehaviour
{
   public void PlayChainThrowSound()
    {
        AudioManager.instance.Play("ThrowChain");
    }
}
