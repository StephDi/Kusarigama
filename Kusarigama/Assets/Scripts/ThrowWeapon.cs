using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
  
    public Transform aimingTarget;
    public Transform weapon;
    public Collider weaponCollider;
    public Transform weaponPoint;

    public Animator anim;

    float throwValue;
    bool weaponIsFlying = false;

    // Update is called once per frame
    void Update()
    {
     
        throwValue = Input.GetAxis("Throw");

        if (Input.GetButton("Fire2"))
        {                                     
            if (throwValue >= 1)
            {
                ThrowWeaponForward();
            }          
        }

        if (Input.GetButtonUp("Fire2"))
        {                                  
           
        }

        if (throwValue < 1 && weaponIsFlying == true)
        {
            PullWeaponBack();
            anim.SetBool("pullBack",true);
        }
    }  

    //Event
    public void ResetWeapon()
    {
        weapon.gameObject.SetActive(true);
        weapon.SetParent(weaponPoint);

        weapon.localRotation = Quaternion.identity;

        weaponCollider.enabled = false;

        anim.SetBool("throwing",false);
        anim.SetBool("pullBack",false);

        weaponIsFlying = false;
    }

    void PullWeaponBack()
    {
        weapon.rotation = weaponPoint.rotation;
        weapon.position = Vector3.Lerp(weapon.position,weaponPoint.position,0.1f);                  
    }

    void ThrowWeaponForward()
    {
        weaponIsFlying = true;
        anim.SetBool("throwing",true);
        weapon.SetParent(null);
        weapon.position = Vector3.Lerp(weapon.position, aimingTarget.position, 0.1f);
    } 
}
