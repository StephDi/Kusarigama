using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public TMP_Text tutName;
    public TMP_Text tutText;

    public GameObject dialogueWindow;
    public GameObject tutorialWindow;

    private Queue<string> sentences;

    private CharMovement charMovement;

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
        nameText.text = dialogue.name;

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
        tutName.text = dialogue.name;

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
        DialogueIsActive = false;
        charMovement.enabled = true;
        dialogueWindow.SetActive(false);
        tutorialWindow.SetActive(false);
        Debug.Log("End");
    }
}
