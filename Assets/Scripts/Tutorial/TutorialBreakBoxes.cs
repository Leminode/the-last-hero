using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TutorialBreakBoxes : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private AudioClip breakSound; // Sound for breaking

    private readonly List<GameObject> _subjects = new();
    private bool _flowStarted;
    private AudioSource _audioSource;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            _subjects.Add(child.gameObject);
        }

        // Initialize AudioSource
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
    }

    public void ChildCollision2D(Collision2D collision)
    {
        if (_flowStarted || !collision.gameObject.Equals(target))
        {
            return;
        }

        _flowStarted = true;

        // Play the breaking sound
        PlayBreakSound();

        // Release constraints and start destruction flow
        _subjects.ForEach(subject =>
        {
            var rb = subject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints2D.None;
            }
        });

        StartCoroutine(Destruct());
    }

    private void PlayBreakSound()
    {
        if (breakSound != null)
        {
            _audioSource.PlayOneShot(breakSound);
        }
    }

    private IEnumerator Destruct()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(target);

        var destroy = true;

        foreach (var subject in _subjects)
        {
            if (destroy)
            {
                Destroy(subject);
            }

            destroy = !destroy;
        }
    }
}
