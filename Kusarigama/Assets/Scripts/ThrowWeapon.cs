using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
  
    public Transform aimingTarget;
    public Transform weapon;
    public Collider weaponCollider;
    public Transform weaponPoint;
    public Transform character;

    public Animator anim;

    public float throwValue;
    public bool weaponIsFlying = false;

    public PullEnemy pullEnemy;
    public PullToAnchor PullToAnchor;

    private void Start()
    {
        pullEnemy = FindObjectOfType<PullEnemy>();
        PullToAnchor = FindObjectOfType<PullToAnchor>();
    }

    // Update is called once per frame
    void Update()
    {
     
        throwValue = Input.GetAxis("Throw");

        if (Input.GetButton("Fire2"))
        {                                     
            if (throwValue > 0)
            {                
                ThrowWeaponForward(); 
            }         
        }
  
        if (throwValue == 0 && weaponIsFlying == true)
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

        weapon.localPosition = weaponPoint.localPosition;
        weapon.localRotation = Quaternion.identity;

        weaponCollider.enabled = false;

        anim.SetBool("throwing",false);
        anim.SetBool("pullBack",false);

        weaponIsFlying = false;

        pullEnemy.hookEnemy = false;
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
        if (PullToAnchor.state != GrappleState.HIT) 
        {
            weapon.position = Vector3.Lerp(weapon.position, aimingTarget.position, 0.1f);
            weapon.rotation = character.rotation;
        }
    } 
}
