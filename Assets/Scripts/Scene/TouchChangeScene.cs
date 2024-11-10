using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchChangeScene : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TouchChangeScene: touch detected");
        
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"TouchChangeScene: changing scene to {sceneName}");

            SceneManager.LoadScene(sceneName);
        }
    }
}
