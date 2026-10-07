using System;
using UnityEngine;

public class CoinWallet : MonoBehaviour
{
    public event Action<int> CoinsChanged;

    private int coins = 0;

    private void OnEnable()
    {
        CoinPickup.Collected += AddCoins;
    }

    private void OnDisable()
    {
        CoinPickup.Collected -= AddCoins;
    }

    private void Start()
    {
        CoinsChanged?.Invoke(coins);
    }

    private void AddCoins(int amount)
    {
        coins += amount;
        CoinsChanged?.Invoke(coins);
    }
}
