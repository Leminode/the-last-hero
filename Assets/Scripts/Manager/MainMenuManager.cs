using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        GameManager.Instance.ContinueLevel();
    }

    public void Credits()
    {
        GameManager.Instance.Credits();
    }

    public void BackToMenu()
    {
        GameManager.Instance.MainMenu();
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
