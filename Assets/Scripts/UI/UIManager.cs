using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Game Over Screen")]
    [SerializeField]
    private GameObject gameOverScreen;

    [Header("Pause Screen")]
    [SerializeField]
    private GameObject pauseScreen;

    private bool isGameOver;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            PauseGame();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverScreen.SetActive(true);
    }

    public void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }

    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }

    public void PauseGame()
    {
        bool pause = !pauseScreen.activeSelf;

        pauseScreen.SetActive(pause);
        GameManager.Instance.PauseGame(pause);
    }
}
