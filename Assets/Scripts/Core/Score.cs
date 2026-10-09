using UnityEngine;
using System;

public class Score : MonoBehaviour
{
    public static event Action<int> ScoreInfo;

    [SerializeField] private int score;

    public void DeclareScore()
    {
        ScoreInfo?.Invoke(score);
    }
}
