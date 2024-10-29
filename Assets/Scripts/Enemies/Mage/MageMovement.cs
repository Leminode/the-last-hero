using UnityEngine;

public class MageMovement : MonoBehaviour
{
    [SerializeField]
    private float directionChangeCooldonw;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform enemy;

    [SerializeField]
    private Animator mageAnimator;

    private Vector3 initialScale;

    int direction = 0;
    private float directionChangeCooldownTimer = Mathf.Infinity;

    private const string walkingTriggerName = "Walking";

    private readonly System.Random random = new();

    private void Awake()
    {
        initialScale = enemy.localScale;
    }

    private void Update()
    {
        directionChangeCooldownTimer += Time.deltaTime;
        if (directionChangeCooldownTimer >= directionChangeCooldonw)
        {
            directionChangeCooldownTimer = 0;
            direction = random.Next(-1, 2);
        }
        Move(direction);
    }

    private void Move(int direction)
    {
        if (direction == 0)
        {
            mageAnimator.SetBool(walkingTriggerName, false);
        }
        else
        {
            mageAnimator.SetBool(walkingTriggerName, true);
            enemy.localScale = new Vector3(
                Mathf.Abs(initialScale.x) * -direction,
                initialScale.y,
                initialScale.z);
        }

        enemy.position = new Vector3(
            enemy.position.x + Time.deltaTime * speed * direction,
            enemy.position.y,
            enemy.position.z);
    }
}
