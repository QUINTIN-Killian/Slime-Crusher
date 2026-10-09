using UnityEngine;

public class PlayerPowerUpsHandler : MonoBehaviour
{
    public bool HasPowerUp { get; private set; } = false;
    public string CurrentPowerUpTag { get; private set; } = "";

    private AudioClip currentRemoveSound;

    public void EquipPowerUp(string pickupTag, AudioClip equipSound, AudioClip removeSound)
    {
        if (equipSound != null)
        {
            AudioSource.PlayClipAtPoint(equipSound, transform.position, 1.0f);
        }

        CurrentPowerUpTag = pickupTag;
        currentRemoveSound = removeSound;
        HasPowerUp = true;
    }

    public void RemovePowerUp()
    {
        if (!HasPowerUp) return;

        if (currentRemoveSound != null)
        {
            AudioSource.PlayClipAtPoint(currentRemoveSound, transform.position, 1.0f);
        }

        HasPowerUp = false;
        CurrentPowerUpTag = "";
        currentRemoveSound = null;
    }
}
