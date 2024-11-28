using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Game Over Screen")]
    [SerializeField]
    private GameObject gameOverScreen;

    [Header("Pause Screen")]
    [SerializeField]
    private GameObject pauseScreen;

    [Header("Win Screen")]
    [SerializeField]
    private GameObject winScreen;

    private bool isScreenActive;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isScreenActive)
        {
            PauseScreen();
        }

        GameManager.Instance.PauseGame(pauseScreen.activeSelf);
    }

    public void GameOver()
    {
        isScreenActive = true;
        gameOverScreen.SetActive(true);
    }

    public void WinScreen()
    {
        isScreenActive = true;
        winScreen.SetActive(true);
    }

    public void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }

    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }

    public void PauseScreen()
    {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
    }

    public void NextLevel()
    {
        GameManager.Instance.NextLevel();
    }
}
