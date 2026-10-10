using UnityEngine;
using System;

public class Score : MonoBehaviour
{
    public static event Action<int> ScoreInfo;

    [SerializeField] private int score = 0;

    public void DeclareScore(int? scoreOverride = null)
    {
        if (scoreOverride.HasValue) ScoreInfo?.Invoke(scoreOverride.Value);
        else ScoreInfo?.Invoke(score);
    }
}
