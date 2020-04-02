using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    public static Scenemanager instance = null;

    public bool changeLvl = false;

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
        StartCoroutine(LoadSynhronously(sceneIndex));
    }

   IEnumerator LoadSynhronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            {
                if (changeLvl == true)
                {
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}
