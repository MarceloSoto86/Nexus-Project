using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Mixer Connections")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("UI Sliders")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (mainMixer != null)
        {
            {
                float currentBGMVolume = mainMixer.GetFloat("BGMVolume", out float bgmVolume) ? bgmVolume : 0f;
                float currentSFXVolume = mainMixer.GetFloat("SFXVolume", out float sfxVolume) ? sfxVolume : 0f;
            }
        }
    }

    public void SetBGMVolume(float sliderValue)
    {
        float clampedValue = Mathf.Max(sliderValue, 0.0001f); // Evita log(0) al establecer el volumen

        // FORMULA LOGARITMICA: 20 * log10(sliderValue)
        float dbVolume = Mathf.Log10(clampedValue) * 20f;

        mainMixer.SetFloat("BGMVolume", dbVolume);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float clampedValue = Mathf.Max(sliderValue, 0.0001f); // Evita log(0) al establecer el volumen
        // FORMULA LOGARITMICA: 20 * log10(sliderValue)
        float dbVolume = Mathf.Log10(clampedValue) * 20f;
        mainMixer.SetFloat("SFXVolume", dbVolume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        ////Debug.log("FullScreen set to: " + isFullScreen);
    }

}
