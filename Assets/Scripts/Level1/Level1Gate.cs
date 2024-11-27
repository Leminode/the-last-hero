using UnityEngine;

public class Level1Gate : MonoBehaviour
{
    [SerializeField]
    private int leverState;
    
    private bool _closedForever;

    public void UpdateState(int ls)
    {
        if (!_closedForever)
        {
            transform.position = ls == leverState ? new Vector2(0, 5f) : new Vector2(0, 0);
        }
    }

    public void CloseGateForever()
    {
        _closedForever = true;
        transform.position = new Vector2(0, 0);
    }
}
