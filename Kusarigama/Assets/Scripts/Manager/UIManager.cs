using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject panel;
    public GameObject character;
    public Cinemachine.CinemachineVirtualCamera menuCam;

    void Awake()
    {
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        panel.SetActive(false);    
    }

    void Update()
    {
        if (Input.GetButtonDown("Exit"))
        {
            FindObjectOfType<AudioManager>().Play("PauseMenuOpen");
            panel.SetActive(!panel.activeInHierarchy);
            character.GetComponent<CharMovement>().enabled = !character.GetComponent<CharMovement>().enabled;
            menuCam.enabled = !menuCam.enabled;
        }    
    }

}
