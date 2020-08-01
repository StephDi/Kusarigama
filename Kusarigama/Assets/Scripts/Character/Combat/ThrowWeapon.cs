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

    

    public float raycastLength;

    public PullEnemy pullEnemy;
    public PullToAnchor PullToAnchor;
    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    private AimWeapon aimWeapon;
    public bool throwingInput;

    private void Start()
    {
        aimWeapon = FindObjectOfType<AimWeapon>();
        pullEnemy = FindObjectOfType<PullEnemy>();
        PullToAnchor = FindObjectOfType<PullToAnchor>();
    }

    void Update()
    {

        if (!aimWeapon.aiming)
        {
            ResetWeapon();
            weapon.position = weaponPoint.position;
            return;
        }

        if (Input.GetAxis("Throw") > 0.1f || Input.GetButton("Fire1"))
        {
            throwingInput = true;
            anim.SetBool("pullBack", false);
        }
        else
        {
            throwingInput = false;
        }

        if (throwingInput && aimWeapon.aiming)
        {
            ThrowWeaponWithRaycast();
        }
            
        if (!throwingInput)
        {
            ResetWeapon();
            anim.SetBool("pullBack",true);          
        }
    }  

    public void ResetWeapon()
    {

        weapon.rotation = weaponPoint.rotation;
        weapon.position = Vector3.Lerp(weapon.position, weaponPoint.position, 0.2f);
        if(Vector3.Distance(weapon.position, weaponPoint.position) <= .3f)
        {
            weapon.SetParent(weaponPoint);
            weapon.position = weaponPoint.position;
        }
        
        weaponCollider.enabled = false;
        anim.SetBool("throwing",false);
        anim.SetBool("pullBack",false);
        pullEnemy.hookEnemy = false;
        AudioManager.instance.Play("PullChainBack");
    }

    //void ThrowWeaponForward()
    //{
    //    Debug.Log("Throw");      
    //    anim.SetBool("throwing",true);
    //    weapon.SetParent(null);
    //    if (PullToAnchor.state != GrappleState.HIT) 
    //    {
    //        weapon.position = Vector3.Lerp(weapon.position, aimingTarget.position, 0.1f);
    //        weapon.rotation = character.rotation;
    //    }
    //} 

    void ThrowWeaponWithRaycast()
    {
        Debug.DrawRay(weaponPoint.position,aimingTarget.position - weaponPoint.position);
        raycastLength = Vector3.Distance(weaponPoint.position,weapon.position);
        Physics.Raycast(weaponPoint.position, aimingTarget.position - weaponPoint.position,out hit, raycastLength, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log(hit.collider);
                pullEnemy.hookedObject = hit.collider.gameObject;
            }
        }
        weapon.SetParent(null);
        anim.SetBool("throwing",true);
        StartCoroutine(ThrowOut());

    }

    IEnumerator ThrowOut()
    {
        weapon.position = Vector3.Lerp(weapon.position, aimingTarget.position, 0.1f);
        weapon.rotation = character.rotation;
        yield return null;
    }
}
