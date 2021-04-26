using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    private bool hasBeenSwung = false;

    public void OnSwung()
    {
        if (!hasBeenSwung)
        {
            hasBeenSwung = true;
            ScoreKeeper.Instance.IncrementScore();
            TimeKeeper.Instance.AddTime();
        }
    }
}
