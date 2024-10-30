using UnityEngine;

public class TouchInstantKill : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>()?.TakeDamage(float.MaxValue);
        }
    }
}
