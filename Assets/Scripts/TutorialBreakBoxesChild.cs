using UnityEngine;

public class TutorialBreakBoxesChild : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.parent.GetComponent<TutorialBreakBoxes>().ChildCollision2D(collision);
    }
}
