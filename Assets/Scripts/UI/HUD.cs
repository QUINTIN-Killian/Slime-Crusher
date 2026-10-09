using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CoinWallet coinWallet;
    [SerializeField] private ScoreListener score;

    [Header("Lives")]
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private string livesPrefix = "Lives: ";

    [Header("Coins")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private string coinsPrefix = "Coins: ";

    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string scorePrefix = "Score: ";

    private void OnEnable()
    {
        playerHealth.HealthChanged += ShowLives;
        coinWallet.CoinsChanged += ShowCoins;
        score.ScoreChanged += ShowScore;
    }

    private void OnDisable()
    {
        playerHealth.HealthChanged -= ShowLives;
        coinWallet.CoinsChanged -= ShowCoins;
        score.ScoreChanged -= ShowScore;
    }

    private void ShowLives(int lives)
    {
        livesText.text = livesPrefix + lives;
    }

    private void ShowCoins(int coins)
    {
        coinsText.text = coinsPrefix + coins;
    }

    private void ShowScore(int score)
    {
        scoreText.text = scorePrefix + score;
    }
}
