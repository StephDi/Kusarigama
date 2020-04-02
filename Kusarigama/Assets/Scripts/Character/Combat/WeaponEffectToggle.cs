using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffectToggle : MonoBehaviour
{

    [SerializeField]
    private GameObject weaponEffects;

    [SerializeField]
    private GameObject weaponEffectsGhost;

    public void PlayWeaponEffects()
    {
        weaponEffects.SetActive(true);
    }

    public void StopWeaponEffects()
    {
        weaponEffects.SetActive(false);
    }
    
    public void PlayWeaponEffectsGhost()
    {
        weaponEffectsGhost.SetActive(true);
    }

    public void StopWeaponEffectsGhost()
    {
        weaponEffectsGhost.SetActive(false);
    }
}
