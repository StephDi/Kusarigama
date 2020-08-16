using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject playerUi;
    public GameObject panel;
    public GameObject endOfGamePanel;
    public GameObject character;
    public GameObject continueButton;
    public GameObject startButton;
    public GameObject backToMainMenuButton;
    public GameObject toolsPanel;
    public Cinemachine.CinemachineVirtualCamera menuCam;
    public bool menuIsActive;

    private bool gameStarted = false;
    private TriggerTutorialAtStart triggerTutorialAtStart;
    private GameObject triggerTutorialAtStartObject;

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
        playerUi = GameObject.Find("UIPlayer");
        panel = GameObject.Find("UIMenu").transform.GetChild(0).gameObject;
        endOfGamePanel = GameObject.Find("UIMenu").transform.GetChild(1).gameObject ;
        character = GameObject.Find("Character");
        menuCam = GameObject.Find("MenuCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        triggerTutorialAtStartObject = GameObject.Find("TutorialTriggerMovement");
        panel.SetActive(true);
        endOfGamePanel.SetActive(true);
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            panel.transform.GetChild(i).gameObject.SetActive(true);
        }       
        continueButton = GameObject.Find("ContinueButton");
        startButton = GameObject.Find("StartButton");
        backToMainMenuButton = GameObject.Find("BackToMainMenuButton");
        toolsPanel = GameObject.Find("ToolsPanel");
        toolsPanel.SetActive(false);
        endOfGamePanel.SetActive(false);
        panel.SetActive(false);

        if (triggerTutorialAtStartObject != null)
        {
            if (triggerTutorialAtStartObject.TryGetComponent(out TriggerTutorialAtStart TutorialAtStart))
            {
                triggerTutorialAtStart = TutorialAtStart;
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == 0 && gameStarted == false)
        {
            continueButton.SetActive(false);
            startButton.SetActive(true);
            OpenStartUiMenu();
        }
        else
        {
            gameStarted = true;
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
            if ((Input.GetButtonDown("Cancel") || Input.GetButtonDown("Exit")) && gameStarted)
            {
                CloseUiMenu();
            }
        }
    }

    void UIFindObjectsOnLevelWasLoaded()
    {
        Debug.Log("EventUI");
        playerUi = GameObject.Find("UIPlayer");
        panel = GameObject.Find("UIMenu").transform.GetChild(0).gameObject;
        character = GameObject.Find("Character");
        menuCam = GameObject.Find("MenuCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        panel.SetActive(true);
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            panel.transform.GetChild(i).gameObject.SetActive(true);
        }
        continueButton = GameObject.Find("ContinueButton");
        startButton = GameObject.Find("StartButton");
        toolsPanel = GameObject.Find("ToolsPanel");
        startButton.SetActive(false);
        endOfGamePanel.SetActive(false);
        toolsPanel.SetActive(false);
        panel.SetActive(false);
    }

    void OpenStartUiMenu()
    {
        panel.SetActive(true);
        playerUi.SetActive(false);
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
        playerUi.SetActive(false);
        character.GetComponent<CharMovement>().enabled = false;
        character.GetComponent<MeleeCombat>().enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
        continueButton.GetComponent<Button>().Select();
        menuCam.enabled = true;
        menuIsActive = true;
    }

    public void OpenEndMenu()
    {
        endOfGamePanel.SetActive(true);
        playerUi.SetActive(false);
        character.GetComponent<CharMovement>().enabled = false;
        character.GetComponent<MeleeCombat>().enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
        backToMainMenuButton.GetComponent<Button>().Select();
        menuCam.enabled = true;
        menuIsActive = true;
    }

    void CloseUiMenu()
    {
        panel.SetActive(false);
        playerUi.SetActive(true);
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

    public void ToolsButton()
    {
        Debug.Log("OpenToolWindow");
        if (!toolsPanel.activeSelf)
        {
            toolsPanel.SetActive(true);
        }
        else
        {
            toolsPanel.SetActive(false);
        }
    }

    public void BackToMainMenu()
    {
        Debug.Log("BackToMainMenu");
        endOfGamePanel.SetActive(false);
        SceneManager.LoadScene(0);
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
