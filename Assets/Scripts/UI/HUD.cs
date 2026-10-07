using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CoinWallet coinWallet;

    [Header("Lives")]
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private string livesPrefix = "Lives: ";

    [Header("Coins")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private string coinsPrefix = "Coins: ";

    private void OnEnable()
    {
        playerHealth.HealthChanged += ShowLives;
        coinWallet.CoinsChanged += ShowCoins;
    }

    private void OnDisable()
    {
        playerHealth.HealthChanged -= ShowLives;
        coinWallet.CoinsChanged -= ShowCoins;
    }

    private void ShowLives(int lives)
    {
        livesText.text = livesPrefix + lives;
    }

    private void ShowCoins(int coins)
    {
        coinsText.text = coinsPrefix + coins;
    }
}
