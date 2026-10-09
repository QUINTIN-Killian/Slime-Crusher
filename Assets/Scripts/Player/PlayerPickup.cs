using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public bool hasPowerUp = false;

    public void HandlePowerUpPickup(string pickupTag)
    {
        if (hasPowerUp) return;

        hasPowerUp = true;
    }
}
