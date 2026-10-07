using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    private const string WinMessage = "Level Complete!";
    private const string LoseMessage = "You Lost!";

    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text endText;

    private void Awake()
    {
        endText.enabled = false;
    }

    private void OnEnable()
    {
        gameManager.GameEnded += Show;
    }

    private void OnDisable()
    {
        gameManager.GameEnded -= Show;
    }

    private void Show(bool hasWon)
    {
        endText.text = hasWon ? WinMessage : LoseMessage;
        endText.enabled = true;
    }
}
