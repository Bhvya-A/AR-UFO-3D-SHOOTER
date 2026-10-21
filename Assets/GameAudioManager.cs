using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance;

    [Header("Background Music")]
    public AudioSource musicSource;
    public AudioClip backgroundMusic;

    [Header("Destroy Sounds")]
    public AudioSource effectsSource;
    public AudioClip ufoDestroySound;
    public AudioClip meteorDestroySound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayUFODestroy()
    {
        if (effectsSource != null && ufoDestroySound != null)
            effectsSource.PlayOneShot(ufoDestroySound);
    }

    public void PlayMeteorDestroy()
    {
        if (effectsSource != null && meteorDestroySound != null)
            effectsSource.PlayOneShot(meteorDestroySound);
    }
}