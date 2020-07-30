using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    public static Scenemanager instance = null;

    public bool changeLvl = false;

    public delegate void UILevelChange();
    public static event UILevelChange UILevelChanges;
    public delegate void DialogueLevelChange();
    public static event DialogueLevelChange DialogueLevelChanges;

    AsyncOperation operation;

    void Awake()
    {

        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadSynchronously(sceneIndex));
    }

   IEnumerator LoadSynchronously(int sceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(sceneIndex);

        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            {
                if (changeLvl == true)
                {
                    changeLvl = false;
                    operation.allowSceneActivation = true;
                    StartCoroutine(WaitForLevelChanged());
                }
            }
            yield return null;
        }
    }

    IEnumerator WaitForLevelChanged()
    {
        yield return null;
        Debug.Log("EventfiredDialogue");
        UILevelChanges();
        Debug.Log("EventfiredUI");
        DialogueLevelChanges();
    }
}
