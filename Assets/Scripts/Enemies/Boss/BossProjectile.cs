using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float damage;

    private float direction;
    private bool hit;
    private BoxCollider2D boxCollider;
    private float lifetime = 0;
    private const float maximumLifetimeDuration = 10;
    private Transform player;
    private Rigidbody2D rigibody;
    private Transform transform;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigibody = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (hit)
            return;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        rigibody.position = new Vector2(
                rigibody.position.x + Time.deltaTime * speed * directionToPlayer.x,
                rigibody.position.y + Time.deltaTime * speed * directionToPlayer.y
                );

        transform.localScale = new Vector3(
            directionToPlayer.x >= 0 ? 1 : -1,
            directionToPlayer.y >= 0 ? 1 : -1,
            1);
        lifetime += Time.deltaTime;
        if (lifetime > maximumLifetimeDuration)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        hit = true;
        boxCollider.enabled = false;
        gameObject.SetActive(false);
        player.GetComponent<Health>().TakeDamage(damage);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        hit = false;
        lifetime = 0;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
