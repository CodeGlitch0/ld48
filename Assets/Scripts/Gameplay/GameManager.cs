using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class GameStartEvent : UnityEvent { };
[Serializable] public class GameOverEvent : UnityEvent { };

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text finalScoreText;

    public GameStartEvent GameStarting;
    public GameStartEvent GameStarted;
    public GameOverEvent GameOver;

    void Start()
    {
        GameStarting?.Invoke();

        Go();
    }

    public void Go()
    {
        GameStarted?.Invoke();
    }

    public void OnTimeIsUp()
    {
        GameOver?.Invoke();
        finalScoreText.text = ScoreKeeper.Instance.Score.ToString();
    }

    public void RestartGame()
    {
        print("restart game");
        if (SceneController.Instance != null)
        {
            SceneController.Instance.BeginNewGame();
        }
    }

    public void ExitGame()
    {
        print("exit game");
        Application.Quit();
    }
}
