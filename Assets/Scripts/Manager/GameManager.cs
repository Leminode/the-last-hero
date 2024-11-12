using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string PlayerPrefLevel = "level";
    private const int MainMenuScene = 0;
    private const int FirstLevelScene = 1;
    private const int LastLevelScene = 2;

    private GameObject _player;
    private Vector3? _startPosition;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        Instance._player = GameObject.FindGameObjectWithTag("Player");
        Instance.SavePlayerPosition();
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

        if (currentScene == LastLevelScene)
        {
            // TODO: change to end
            Debug.Log($"GameManager: last level {currentScene}, changing to ending");

            return;
        }

        var nextScene = currentScene + 1;

        Debug.Log($"GameManager: next level, changing scene to {nextScene}");

        PlayerPrefs.SetInt(PlayerPrefLevel, nextScene);
        PlayerPrefs.Save();

        SceneManager.LoadScene(nextScene);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }

    public void ResetPlayerPosition()
    {
        if (_startPosition is null || _player.IsUnityNull())
        {
            return;
        }

        var position = _player.transform.position;

        Debug.Log($"GameManager: resetting player position {position.ToString()}");

        _player.transform.position = _startPosition.Value;
    }

    private void SavePlayerPosition()
    {
        if (_player.IsUnityNull())
        {
            return;
        }

        var position = _player.transform.position;

        Debug.Log($"GameManager: saving player position {position.ToString()}");

        _startPosition = position;
    }
}
