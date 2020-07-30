using UnityEngine;
using UnityEngine.SceneManagement;

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
    private void OnEnable()
    {       

        Scenemanager.UILevelChanges += UIFindObjectsOnLevelWasLoaded;
    }

    private void OnDisable()
    {

        Scenemanager.UILevelChanges += UIFindObjectsOnLevelWasLoaded;
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
            UpdateUiState();
        }

        if (panel != null)
        {
            if (panel.activeSelf == true && Input.GetButtonDown("Cancel"))
            {
                UpdateUiState();
            }
        }

    }

    void UIFindObjectsOnLevelWasLoaded()
    {
        Debug.Log("EventUI");
        panel = GameObject.Find("UIMenu").transform.GetChild(0).gameObject;
        character = GameObject.Find("Character");
        menuCam = GameObject.Find("MenuCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    void UpdateUiState()
    {
        panel.SetActive(!panel.activeInHierarchy);
        character.GetComponent<CharMovement>().enabled = !character.GetComponent<CharMovement>().enabled;
        menuCam.enabled = !menuCam.enabled;
    }
}
