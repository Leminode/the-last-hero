using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBreakBoxes : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private AudioClip breakSound; // Sound to play when boxes break

    private AudioSource audioSource; // To play the sound

    private readonly List<GameObject> _subjects = new();
    private bool _flowStarted;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            _subjects.Add(child.gameObject);
        }

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure AudioSource properties
        audioSource.playOnAwake = false;
    }

    public void ChildCollision2D(Collision2D collision)
    {
        if (_flowStarted || !collision.gameObject.Equals(target))
        {
            return;
        }

        _flowStarted = true;

        // Play the breaking sound when destruction begins
        PlayBreakSound();

        _subjects.ForEach(delegate (GameObject subject)
        {
            subject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        });

        StartCoroutine(Destruct());
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

    private void PlayBreakSound()
    {
        if (breakSound != null && audioSource != null)
        {
            audioSource.clip = breakSound;
            audioSource.Play();
        }
    }
}
