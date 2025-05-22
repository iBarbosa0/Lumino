using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Text sfxVolumeText;
    public Text musicVolumeText;
    public AudioSource backgroundMusic; // Atribui o AudioSource da música de fundo
    public AudioSource sfxSource;

    private void Start()
    {
        // SFX
        float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f); // Carrega o volume salvo ou usa 1f por padrão
        sfxVolumeSlider.value = savedSfxVolume;
        SetSFXVolume(savedSfxVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume); // Liga o evento do slider

        // Music
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicVolumeSlider.value = savedMusicVolume;
        SetMusicVolume(savedMusicVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

        settingsPanel.SetActive(false); // Começa escondido
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void SetSFXVolume(float value)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = value;
        }

        PlayerPrefs.SetFloat("SFXVolume", value);
        if (sfxVolumeText != null)
        {
            sfxVolumeText.text = Mathf.RoundToInt(value * 100) + "%"; // Atualiza o texto de percentagem   
        }
    }

    public void SetMusicVolume(float value)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = value;
        }

        PlayerPrefs.SetFloat("MusicVolume", value);
        if (musicVolumeText != null)
        {
            musicVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
        }
    }

}
