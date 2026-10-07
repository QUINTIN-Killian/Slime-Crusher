using UnityEngine;
using System;

public class WinFlag : MonoBehaviour
{
    public event Action Reached;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            Reached?.Invoke();
        }
    }
}
