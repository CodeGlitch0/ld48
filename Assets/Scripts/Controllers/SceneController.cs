using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable] public class SceneOperationEvent : UnityEvent { };

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [SerializeField] private string gameSceneName;

    private Scene gameScene;
    private bool isGameLoaded = false;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        BeginNewGame();
    }

    public void BeginNewGame()
    {
        StartCoroutine(LoadAndStartGameScene());
    }

    private IEnumerator LoadAndStartGameScene()
    {
        if (isGameLoaded)
        {
            var gameUnloader = SceneManager.UnloadSceneAsync(gameScene);

            while (!gameUnloader.isDone)
            {
                yield return null;
            }

            isGameLoaded = false;
        }

        var gameLoader = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);

        while (!gameLoader.isDone)
        {
            yield return null;
        }

        gameScene = SceneManager.GetSceneByName(gameSceneName);
        SceneManager.SetActiveScene(gameScene);

        isGameLoaded = true;
    }

    private void EndGame()
    {
        Application.Quit();
    }
}
