using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoxHealth : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField]private Image healthBarFill;


    private void Start()
    {
        mainCamera = Camera.main;
        healthBarFill.transform.parent.GetComponent<Canvas>().enabled = false;
    }

    private void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
    }

    public void UpdateUI(float health)
    {    
        healthBarFill.transform.parent.GetComponent<Canvas>().enabled = true;
        healthBarFill.fillAmount = health / 30f;
    }
}
