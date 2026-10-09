using UnityEngine;

public class PlayerPowerUpsHandler : MonoBehaviour
{
    public bool HasPowerUp { get; private set; } = false;
    public string CurrentPowerUpTag { get; private set; } = "";
    public string AnimationTrigger { get; private set; } = "";
    public AThrowable throwObject;

    private AudioClip currentRemoveSound;

    public void EquipPowerUp(string pickupTag, string animationTrigger, AThrowable throwable, AudioClip equipSound, AudioClip removeSound)
    {
        if (equipSound != null)
        {
            AudioSource.PlayClipAtPoint(equipSound, transform.position, 1.0f);
        }

        HasPowerUp = true;
        CurrentPowerUpTag = pickupTag;
        AnimationTrigger = animationTrigger;
        throwObject = throwable;
        currentRemoveSound = removeSound;
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
        throwObject = null;
        currentRemoveSound = null;
    }
}
