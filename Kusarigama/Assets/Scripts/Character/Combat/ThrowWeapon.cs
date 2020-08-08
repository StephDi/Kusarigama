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
    public PullToAnchor pullToAnchor;
    [SerializeField] private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    private AimWeapon aimWeapon;
    public bool throwingInput;
    private bool weaponIsFlying = false;

    private void Start()
    {
        aimWeapon = FindObjectOfType<AimWeapon>();
        pullEnemy = FindObjectOfType<PullEnemy>();
        pullToAnchor = FindObjectOfType<PullToAnchor>();
    }

    void Update()
    {

        if (!aimWeapon.aiming && weaponIsFlying)
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
            weaponIsFlying = true;
            ThrowWeaponWithRaycast();
        }
            
        if (!throwingInput && weaponIsFlying)
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
            weaponIsFlying = false;
        }
        
        weaponCollider.enabled = false;
        anim.SetBool("throwing",false);
        anim.SetBool("pullBack",false);
        pullEnemy.hookEnemy = false;
    }

    void ThrowWeaponWithRaycast()
    {
        Debug.DrawRay(weaponPoint.position, aimingTarget.position - weaponPoint.position);
        raycastLength = Vector3.Distance(weaponPoint.position, weapon.position);
        Physics.Raycast(weaponPoint.position, aimingTarget.position - weaponPoint.position,out hit, raycastLength, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemyhit");
                pullEnemy.hookedObject = hit.collider.gameObject;
            }
            if (hit.collider.CompareTag("Anchor"))
            {
                Debug.Log("Anchorhit");
                pullToAnchor.state = GrappleState.HIT;
                if (hit.collider != pullToAnchor.hookedObject)
                {
                    pullToAnchor.lastHookedObject = pullToAnchor.hookedObject;
                    pullToAnchor.hookedObject = hit.collider.gameObject;
                    pullToAnchor.hookedObject.layer = 2;
                }
            }
        }

        weapon.SetParent(null);
        anim.SetBool("throwing", true);
        StartCoroutine(ThrowOut());

    }

    IEnumerator ThrowOut()
    {
        weapon.position = Vector3.Lerp(weapon.position, aimingTarget.position, 0.1f);
        weapon.rotation = character.rotation;
        yield return null;
    }
}
