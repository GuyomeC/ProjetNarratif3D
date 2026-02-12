using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("----- Audio Sources -----")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("----- Audio Clips -----")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip Scene1Music;
    [SerializeField] private AudioClip Scene2Music;
    [SerializeField] private AudioClip Scene3Music;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip dialogueSFX;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Audio");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        musicAudioSource.clip = mainMenuMusic;
        musicAudioSource.Play();
    }

    public void PlayOneShot(AudioClip audio)
    {
        sfxAudioSource.PlayOneShot(audio);
    }
}
