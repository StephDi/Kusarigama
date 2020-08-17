using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthCheatUI : MonoBehaviour
{
    private Camera mainCamera;
    private TMP_Text healthCheatText;
    private CharacterHealth characterHealth;
    private GameObject HealthCheatPanel;

    private void Awake()
    {
        mainCamera = Camera.main;
        healthCheatText = GetComponentInChildren<TMP_Text>();
        characterHealth = FindObjectOfType<CharacterHealth>();
        HealthCheatPanel = GameObject.Find("HealthCheatUI").transform.GetChild(0).gameObject;
        HealthCheatPanel.SetActive(false);
    }

    private void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
    }

    public void DisplayHealthCheatText()
    {
        HealthCheatPanel.SetActive(true);
        healthCheatText.text = characterHealth.maxHealth + "health";
        StartCoroutine(HidePanelAfterTime());
    }

    IEnumerator HidePanelAfterTime()
    {
        yield return new WaitForSeconds(2f);
        if (HealthCheatPanel.activeSelf)
        {
            HealthCheatPanel.SetActive(false);
        }
    }
}
