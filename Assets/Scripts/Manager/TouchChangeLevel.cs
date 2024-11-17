using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchChangeLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TouchChangeScene: touch detected");

        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.NextLevel();
        }
    }
}
