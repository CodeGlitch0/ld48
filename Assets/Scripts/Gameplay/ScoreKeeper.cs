using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Instance;

    [Header("UI References")]
    public TMP_Text scoreText;

    private int currentScore = 0;

    public int Score { get => currentScore; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        Reset();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void Reset()
    {
        currentScore = 0;
        scoreText.text = "0";
    }

    public void IncrementScore()
    {
        currentScore++;
        scoreText.text = currentScore.ToString();
    }
}
