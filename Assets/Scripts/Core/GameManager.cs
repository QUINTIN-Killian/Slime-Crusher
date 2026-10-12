using System;
using UnityEngine;

public enum GameState { Playing, Won, Lost }

public class GameManager : MonoBehaviour
{
    public event Action<bool> GameEnded;

    private GameState state = GameState.Playing;
    private static Vector3? respawnPosition;

    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private WinFlag winFlag;
    [SerializeField] private BossDeath boss;
    [SerializeField] private Timer timer;

    private void OnEnable()
    {
        playerHealth.Died += HandlePlayerDied;
        CheckpointFlag.Reached += HandleCheckpointReached;
        winFlag.Reached += Win;
        boss.BossKilled += Win;
        timer.TimerEnd += Lose;
    }

    private void Start()
    {
        if (respawnPosition.HasValue)
        {
            player.position = respawnPosition.Value;
        }
    }

    private void OnDisable()
    {
        playerHealth.Died -= HandlePlayerDied;
        CheckpointFlag.Reached -= HandleCheckpointReached;
        winFlag.Reached -= Win;
        boss.BossKilled -= Win;
        timer.TimerEnd -= Lose;
    }

    private void HandlePlayerDied()
    {
        EndGame(false);
    }

    private void HandleCheckpointReached(Vector3 position)
    {
        respawnPosition = position;
    }

    private void Win()
    {
        if (playerHealth.IsDead) return;

        EndGame(true);
    }

    private void Lose()
    {
        if (playerHealth.IsDead) return;

        EndGame(false);
    }

    private void EndGame(bool hasWon)
    {
        if (state != GameState.Playing) return;

        if (hasWon)
        {
            respawnPosition = null;
            state = GameState.Won;
        }
        else
        {
            state = GameState.Lost;
        }

        // Time.timeScale = 0f;
        GameEnded?.Invoke(hasWon);
    }
}
