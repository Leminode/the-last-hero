using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string PlayerPrefLevel = "level";
    private const int MainMenuScene = 0;
    private const int CreditsScene = 1;
    private const int FirstLevelScene = 2;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

            return;
        }

        Instance = this;
    }

    public void ContinueLevel()
    {
        var scene = PlayerPrefs.GetInt(PlayerPrefLevel, FirstLevelScene);

        Debug.Log($"GameManager: continue level, changing scene to {scene}");

        SceneManager.LoadScene(scene);
    }

    public void NextLevel()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        var nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = MainMenuScene;

            PlayerPrefs.SetInt(PlayerPrefLevel, FirstLevelScene);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefLevel, nextScene);
        }
        
        Debug.Log($"GameManager: next level, changing scene to {nextScene}");

        PlayerPrefs.Save();

        SceneManager.LoadScene(nextScene);
    }

    public void RestartLevel()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;

        Debug.Log($"GameManager: restart level, changing scene to {currentScene}");

        SceneManager.LoadScene(currentScene);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }

    public void Credits()
    {
        SceneManager.LoadScene(CreditsScene);
    }

    public void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
}
