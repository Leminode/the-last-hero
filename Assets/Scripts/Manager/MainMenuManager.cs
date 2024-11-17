using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        GameManager.Instance.ContinueLevel();
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
