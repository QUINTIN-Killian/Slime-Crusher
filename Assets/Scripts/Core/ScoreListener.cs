using System;
using UnityEngine;

public class ScoreListener : MonoBehaviour
{
    public event Action<int> ScoreChanged;

    private int score = 0;

    private void OnEnable()
    {
        Score.ScoreInfo += UpdateScore;
    }

    private void Start()
    {
        ScoreChanged?.Invoke(score);
    }

    private void OnDisable()
    {
        Score.ScoreInfo -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        this.score += score;
        ScoreChanged?.Invoke(this.score);
    }
}
