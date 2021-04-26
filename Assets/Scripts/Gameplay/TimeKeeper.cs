using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class TimesUpEvent : UnityEvent { };
[Serializable] public class TimeAddedEvent : UnityEvent<int> { };

public class TimeKeeper : MonoBehaviour
{
    public static TimeKeeper Instance;

    [SerializeField] private float secondsPerRound = 60f;
    [SerializeField] private float timeAddedPerVine = 0f;

    [Header("UI References")]
    public TMP_Text timeRemainingText;

    [Header("Timer Events")]
    public TimeAddedEvent OnTimeAdded;
    public TimesUpEvent OnTimeIsUp;

    private float timeRemaining = 0f;

    public float TimeRemaining { get => timeRemaining; }

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

    public void BeginTimer()
    {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        Reset();

        float delta = 0f;

        int fullSecondsRemaining = Mathf.CeilToInt(timeRemaining);
        int lastfullSecondsRemaining = fullSecondsRemaining;

        timeRemainingText.text = fullSecondsRemaining.ToString();

        while (timeRemaining > 0f)
        {
            yield return null; // Until next frame

            delta = Time.deltaTime;
            timeRemaining = timeRemaining - delta;

            if (timeRemaining < 0f)
            {
                timeRemaining = 0f;
            }

            fullSecondsRemaining = Mathf.CeilToInt(timeRemaining);
            if (fullSecondsRemaining != lastfullSecondsRemaining)
            {
                lastfullSecondsRemaining = fullSecondsRemaining;
                timeRemainingText.text = fullSecondsRemaining.ToString();
            }
        }

        OnTimeIsUp?.Invoke();
    }

    public void Reset()
    {
        timeRemaining = secondsPerRound;
        timeRemainingText.text = timeRemaining.ToString();
    }

    public void AddTime()
    {
        timeRemaining = timeRemaining + timeAddedPerVine;
    }
}
