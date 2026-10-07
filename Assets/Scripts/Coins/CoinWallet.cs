using System;
using UnityEngine;

public class CoinWallet : MonoBehaviour
{
    public event Action<int> CoinsChanged;

    private int coins = 0;

    private void Start()
    {
        CoinsChanged?.Invoke(coins);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        CoinsChanged?.Invoke(coins);
    }
}
