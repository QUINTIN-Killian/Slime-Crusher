using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    private const string WinMessage = "Level Complete!";
    private const string LoseMessage = "You Lost!";

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private TMP_Text endText;

    private void Awake()
    {
        gameOverMenu.SetActive(false);
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
        gameOverMenu.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
