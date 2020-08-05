using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState {NONE, MELEE, MELEECHAIN, MELEEGHOST, MELEECHAINGHOST}

public class GetWeaponUpgrades : MonoBehaviour
{
    //SCRIPTS
    private MeleeCombat meleeCombat;
    private RangedCombat rangedCombat;
    private AimWeapon aimWeapon;
    private ThrowWeapon throwWeapon;
    private RangedCombatGhost rangedCombatGhost;

    public WeaponState weaponState = WeaponState.NONE;

    //WEAPONS
    [SerializeField]
    private GameObject kusarigama;
    [SerializeField]
    private GameObject kusarigamaGhost;

    //COLLECTABLES
    [SerializeField]
    private GameObject kusarigamaCollectable;
    [SerializeField]
    private GameObject ChainCollectable;
    [SerializeField]
    private GameObject kusarigamaGhostCollectable;

    private void Start()
    {    
        meleeCombat = GetComponent<MeleeCombat>();
        rangedCombat = GetComponent<RangedCombat>();
        aimWeapon = GetComponent<AimWeapon>();
        throwWeapon = GetComponent<ThrowWeapon>();
        rangedCombatGhost = GetComponent<RangedCombatGhost>();

        meleeCombat.enabled = false;
        rangedCombat.enabled = false;
        aimWeapon.enabled = false;
        throwWeapon.enabled = false;
        rangedCombatGhost.enabled = false;

        kusarigama.SetActive(false);
        kusarigamaGhost.SetActive(false);

        GetWeaponState();
    }

    void GetWeaponState()
    {
        switch (weaponState)
        {
            case WeaponState.NONE:
                break;

            case WeaponState.MELEE:
                if(kusarigamaCollectable != null)
                { 
                    kusarigamaCollectable.SetActive(false);
                }
                kusarigama.SetActive(true);
                meleeCombat.enabled = true;
                break;

            case WeaponState.MELEECHAIN:
                if (ChainCollectable != null)
                {
                    ChainCollectable.SetActive(false);
                }
                kusarigama.SetActive(true);
                meleeCombat.enabled = true;
                rangedCombat.enabled = true;
                aimWeapon.enabled = true;
                throwWeapon.enabled = true;
                break;

            case WeaponState.MELEEGHOST:
                if (kusarigamaGhostCollectable != null)
                {
                    kusarigamaGhostCollectable.SetActive(false);
                }
                kusarigamaGhost.SetActive(true);
                meleeCombat.enabled = true;
                rangedCombatGhost.enabled = true;
                break;

            case WeaponState.MELEECHAINGHOST:
                kusarigama.SetActive(true);
                kusarigamaGhost.SetActive(true);
                meleeCombat.enabled = true;
                rangedCombat.enabled = true;
                aimWeapon.enabled = true;
                throwWeapon.enabled = true;
                rangedCombatGhost.enabled = true;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MeleeUpgrade" )
        {
            weaponState = WeaponState.MELEE;
            GetWeaponState();
        }

        if (other.tag == "ChainUpgrade")
        {
            if (weaponState == WeaponState.MELEE)
            {
                weaponState = WeaponState.MELEECHAIN;
            }
            else
            {
                weaponState = WeaponState.MELEECHAINGHOST;
            }
            GetWeaponState();
        }
        if (other.tag == "GhostUpgrade")
        {
            if (weaponState == WeaponState.MELEE)
            {
                weaponState = WeaponState.MELEEGHOST;
            }
            else
            {
                weaponState = WeaponState.MELEECHAINGHOST;
            }
            GetWeaponState();
        }
    }
}
