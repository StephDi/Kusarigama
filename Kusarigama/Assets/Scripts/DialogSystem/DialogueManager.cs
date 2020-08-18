using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    //public TMP_Text dialogueName;
    public TMP_Text dialogueText;

    //public TMP_Text tutName;
    public TMP_Text tutText;

    public GameObject dialogueWindow;
    public GameObject tutorialWindow;

    private Queue<string> sentences;

    private CharMovement charMovement;

    private GameObject UIDialogue;

    public bool DialogueIsActive;
    private void Awake()
    {
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        charMovement = FindObjectOfType<CharMovement>();
        sentences = new Queue<string>();
        UIDialogue = GameObject.Find("UIDialog");
    }

    private void OnLevelWasLoaded(int level)
    {
        charMovement = FindObjectOfType<CharMovement>();
        UIDialogue = GameObject.Find("UIDialog");
        dialogueWindow = UIDialogue.transform.GetChild(0).gameObject;
        tutorialWindow = UIDialogue.transform.GetChild(1).gameObject;
        //dialogueName = dialogueWindow.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        dialogueText = dialogueWindow.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        //tutName = tutorialWindow.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        tutText = tutorialWindow.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        Scenemanager.DialogueLevelChanges += DialogueFindObjectsOnLevelWasLoaded;
    }

    private void OnDisable()
    {
        Scenemanager.DialogueLevelChanges -= DialogueFindObjectsOnLevelWasLoaded;
    }

    private void DialogueFindObjectsOnLevelWasLoaded()
    {
        Debug.Log("EventDialogue");
        charMovement = FindObjectOfType<CharMovement>();
        UIDialogue = GameObject.Find("UIDialog");
        dialogueWindow = UIDialogue.transform.GetChild(0).gameObject;
        tutorialWindow = UIDialogue.transform.GetChild(1).gameObject;
        //dialogueName = dialogueWindow.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        dialogueText = dialogueWindow.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        //tutName = tutorialWindow.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        tutText = tutorialWindow.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialog( Dialogue dialogue)
    {
        DialogueIsActive = true;
        //dialogueName.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }  
        
        charMovement.anim.SetBool("moving",false);
        charMovement.enabled = false;
        dialogueWindow.SetActive(true);
        DisplayNextSentence();
    }
    public void StartTutorial( Dialogue dialogue)
    {
        DialogueIsActive = true;
        //tutName.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        charMovement.anim.SetBool("moving",false);
        charMovement.enabled = false;
        tutorialWindow.SetActive(true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        tutText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            tutText.text += letter;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        if (DialogueIsActive == true)
        {
            DialogueIsActive = false;
            charMovement.enabled = true;
            dialogueWindow.SetActive(false);
            tutorialWindow.SetActive(false);
            Debug.Log("EndDialogue");
        }
    }
}
