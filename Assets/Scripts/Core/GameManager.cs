using System;
using UnityEngine;

public enum GameState { Playing, Won, Lost }

public class GameManager : MonoBehaviour
{
    public event Action<bool> GameEnded;

    private GameState state = GameState.Playing;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private WinFlag winFlag;

    private void OnEnable()
    {
        playerHealth.Died += HandlePlayerDied;
        winFlag.Reached += HandleFlagReached;
    }

    private void OnDisable()
    {
        playerHealth.Died -= HandlePlayerDied;
        winFlag.Reached -= HandleFlagReached;
    }

    private void HandlePlayerDied()
    {
        EndGame(false);
    }

    private void HandleFlagReached()
    {
        if (playerHealth.IsDead) return;

        EndGame(true);
    }

    private void EndGame(bool hasWon)
    {
        if (state != GameState.Playing) return;

        state = hasWon ? GameState.Won : GameState.Lost;
        Time.timeScale = 0f;
        GameEnded?.Invoke(hasWon);
    }
}
