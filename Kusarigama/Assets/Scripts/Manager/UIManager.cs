using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject panel;
    public GameObject character;
    public GameObject continueButton;
    public GameObject startButton;
    public Cinemachine.CinemachineVirtualCamera menuCam;
    public bool menuIsActive;

    private bool gameStarted = false;
    private TriggerTutorialAtStart triggerTutorialAtStart;

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
        panel.SetActive(true);
        continueButton = GameObject.Find("ContinueButton");
        startButton = GameObject.Find("StartButton");
        panel.SetActive(false);
        panel = GameObject.Find("UIMenu").transform.GetChild(0).gameObject;
        character = GameObject.Find("Character");
        menuCam = GameObject.Find("MenuCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if(GameObject.Find("TutorialTriggerMovement").TryGetComponent(out TriggerTutorialAtStart TutorialAtStart))
        {
            triggerTutorialAtStart = TutorialAtStart;   
        }

        if (SceneManager.GetActiveScene().buildIndex == 0 && gameStarted == false)
        {
            continueButton.SetActive(false);
            startButton.SetActive(true);
            OpenStartUiMenu();
        }
    }

    void Update()
    {
        if (!panel.activeSelf && !DialogueManager.instance.DialogueIsActive)
        {
            if (Input.GetButtonDown("Exit"))
            {

                continueButton.SetActive(true);
                startButton.SetActive(false);
                OpenUiMenu();
                FindObjectOfType<AudioManager>().Play("PauseMenuOpen");
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Exit"))
            {
                CloseUiMenu();
            }
        }
    }

    void UIFindObjectsOnLevelWasLoaded()
    {
        Debug.Log("EventUI");
        panel = GameObject.Find("UIMenu").transform.GetChild(0).gameObject;
        character = GameObject.Find("Character");
        menuCam = GameObject.Find("MenuCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        panel.SetActive(true);
        continueButton = GameObject.Find("ContinueButton");
        panel.SetActive(false);
    }

    void OpenStartUiMenu()
    {
        panel.SetActive(true);
        character.GetComponent<CharMovement>().enabled = false;
        character.GetComponent<MeleeCombat>().enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
        startButton.GetComponent<Button>().Select();
        menuCam.enabled = true;
        menuIsActive = true;
    }

    void OpenUiMenu()
    {
        panel.SetActive(true);
        character.GetComponent<CharMovement>().enabled = false;
        character.GetComponent<MeleeCombat>().enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
        continueButton.GetComponent<Button>().Select();
        menuCam.enabled = true;
        menuIsActive = true;
    }

    void CloseUiMenu()
    {
        panel.SetActive(false);
        character.GetComponent<CharMovement>().enabled = true;
        character.GetComponent<MeleeCombat>().enabled = true;
        menuCam.enabled = false;
        menuIsActive = false;
    }

    public void StartButton()
    {
        gameStarted = true;
        Debug.Log("Start");
        if (triggerTutorialAtStart != null)
        {
            CloseUiMenu();
            StartCoroutine(StartGameAfterTime());
        }

    }

    public void Continue()
    {
        CloseUiMenu();
    }

    public void Exit()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    public IEnumerator StartGameAfterTime()
    {
        yield return new WaitForSeconds(1f);
        triggerTutorialAtStart.StartTutorial();
    }
}
