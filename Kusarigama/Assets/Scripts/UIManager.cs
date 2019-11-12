using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Update()
    {
        if (Input.GetButtonDown("Exit"))
        {
            panel.SetActive(!panel.activeInHierarchy);
            character.GetComponent<CharMovement>().enabled = !character.GetComponent<CharMovement>().enabled;
            menuCam.enabled = !menuCam.enabled;
        }    
    }

}
