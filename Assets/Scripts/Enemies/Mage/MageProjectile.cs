using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float direction;
    private bool hit;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private float lifetime = 0;
    private const float maximumLifetimeDuration = 10;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit)
            return;

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > maximumLifetimeDuration)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        gameObject.SetActive(false);
        // Explode?
    }
    public void SetDirection(float newDirection)
    {
        direction = newDirection;
        gameObject.SetActive(true);
        hit = false;
        lifetime = 0;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != newDirection)
            localScaleX = newDirection;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
 