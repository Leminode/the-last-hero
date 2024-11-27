using UnityEngine;
using UnityEngine.Events;

public class CloseGate : MonoBehaviour
{
    public UnityEvent closeGate;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        closeGate.Invoke();
    }
}
