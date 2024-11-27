using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string PlayerPrefLevel = "level";
    private const int MainMenuScene = 0;
    private const int FirstLevelScene = 1;
    private const int EndGameScene = 3;

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

        Debug.Log($"GameManager: next level, changing scene to {nextScene}");
        PlayerPrefs.SetInt(PlayerPrefLevel, nextScene);

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
        Debug.Log($"GameManager: main MENU");
        SceneManager.LoadScene(MainMenuScene);
    }
    
    public void EndGame()
    {
        StartCoroutine(startEndGame());
    }

    private IEnumerator startEndGame()
    {
        yield return new WaitForSeconds(0.5f);
        var l = GameObject.FindGameObjectWithTag("Light").GetComponent<Light2D>();
        
        while (l.intensity > 0)
        { 
            l.intensity -= 0.005f;
            yield return new WaitForSeconds(0.001f);
        }

        PlayerPrefs.SetInt(PlayerPrefLevel, FirstLevelScene);
        SceneManager.LoadScene(EndGameScene);
    }
}
