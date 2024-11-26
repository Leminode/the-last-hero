using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public static PlayerSoundManager instance { get; private set; }

    private AudioSource source;

    private void Awake()
    {
        // Singleton pattern, do not try to reuse on other objects
        instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
