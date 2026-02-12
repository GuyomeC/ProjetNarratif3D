using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSLider;
    [SerializeField] private Slider TextSpeedSlider;
    public float textSpeed;
    public TMP_Dropdown LanguageDropdown;
    public bool IsInPause = false;


    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MenuPause");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            LoadMusicVolume();
            LoadTextSpeed();
        }
        else
        {
            musicSlider.value = 0.5f;
            SFXSLider.value = 0.5f;
            TextSpeedSlider.value = 0.5f;
            SetMusicVolume();
            SetSFXVolume();
            SetTextSpeed();
        }

    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    
    public void SetSFXVolume()
    {
        float volume = SFXSLider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSLider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetTextSpeed()
    {
        textSpeed = (1-TextSpeedSlider.value) / 10;
        PlayerPrefs.SetFloat("textSpeed", textSpeed);
    }

    public void LoadTextSpeed ()
    {
        textSpeed = PlayerPrefs.GetFloat("textSpeed", 0.05f);
        SetTextSpeed();
    }

    public void TogglePause()
    {
        IsInPause = !IsInPause;
    }
}
