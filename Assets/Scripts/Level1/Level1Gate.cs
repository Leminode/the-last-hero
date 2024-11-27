using UnityEngine;

public class Level1Gate : MonoBehaviour
{
    public void UpdateState(int leverState)
    {
        transform.position = leverState == 0 ? new Vector2(0, 5f) : new Vector2(0, 0);
    }
}
