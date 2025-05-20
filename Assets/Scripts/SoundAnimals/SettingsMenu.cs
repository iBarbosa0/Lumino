using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public Text volumePercentText;

    private void Start()
    {
        // Carrega o volume salvo ou usa 1f por padrão
        float savedVolume = PlayerPrefs.GetFloat("MinigameVolume", 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Liga o evento do slider
        volumeSlider.onValueChanged.AddListener(SetVolume);

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

    public void SetVolume(float value)
    {
        AudioListener.volume = value; // Controla o volume geral
        PlayerPrefs.SetFloat("MinigameVolume", value);

        // Atualiza o texto de percentagem
        if (volumePercentText != null)
        {
            int percent = Mathf.RoundToInt(value * 100);
            volumePercentText.text = percent + "%";
        }
    }
}
