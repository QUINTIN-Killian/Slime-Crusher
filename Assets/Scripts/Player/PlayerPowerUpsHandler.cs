using UnityEngine;

public class PlayerPowerUpsHandler : MonoBehaviour
{
    public bool HasPowerUp { get; private set; } = false;
    public string CurrentPowerUpTag { get; private set; } = "";
    public string AnimationTrigger { get; private set; } = "";

    private AudioClip currentRemoveSound;

    public void EquipPowerUp(string pickupTag, string animationTrigger, AudioClip equipSound, AudioClip removeSound)
    {
        if (equipSound != null)
        {
            AudioSource.PlayClipAtPoint(equipSound, transform.position, 1.0f);
        }

        CurrentPowerUpTag = pickupTag;
        AnimationTrigger = animationTrigger;
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
        AnimationTrigger = "";
        currentRemoveSound = null;
    }
}
