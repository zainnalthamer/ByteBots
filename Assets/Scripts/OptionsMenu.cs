using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    [Header("Graphics")]
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown modeDropdown;

    private Resolution[] resolutions;
    private FullScreenMode currentMode;

    void Start()
    {
        graphicsDropdown.ClearOptions();
        graphicsDropdown.AddOptions(new List<string>(QualitySettings.names));
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        modeDropdown.ClearOptions();
        List<string> modes = new List<string>() { "Fullscreen", "Windowed", "Borderless" };
        modeDropdown.AddOptions(modes);
        currentMode = Screen.fullScreenMode;
        modeDropdown.value = PlayerPrefs.GetInt("DisplayMode", ModeToIndex(currentMode));
        modeDropdown.RefreshShownValue();

        ApplyResolution();
        ApplyDisplayMode();

        if (masterVolumeSlider != null)
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVol", 1f);
        if (musicVolumeSlider != null)
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVol", 1f);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MasterVol", value);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MusicVol", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("SFXVol", value);
    }

    public void SetGraphicsQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetResolution(int index)
    {
        PlayerPrefs.SetInt("ResolutionIndex", index);
        ApplyResolution();
    }

    private void ApplyResolution()
    {
        Resolution resolution = resolutions[PlayerPrefs.GetInt("ResolutionIndex", resolutions.Length - 1)];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    public void SetDisplayMode(int index)
    {
        PlayerPrefs.SetInt("DisplayMode", index);
        ApplyDisplayMode();
    }

    private void ApplyDisplayMode()
    {
        FullScreenMode mode = IndexToMode(PlayerPrefs.GetInt("DisplayMode", 0));
        Screen.fullScreenMode = mode;
    }

    private int ModeToIndex(FullScreenMode mode)
    {
        switch (mode)
        {
            case FullScreenMode.FullScreenWindow: return 0;
            case FullScreenMode.Windowed: return 1;
            case FullScreenMode.ExclusiveFullScreen: return 2;
            default: return 0;
        }
    }

    private FullScreenMode IndexToMode(int index)
    {
        switch (index)
        {
            case 0: return FullScreenMode.FullScreenWindow;
            case 1: return FullScreenMode.Windowed;
            case 2: return FullScreenMode.ExclusiveFullScreen;
            default: return FullScreenMode.FullScreenWindow;
        }
    }

    public void BackToMenu(GameObject optionsPanel)
    {
        optionsPanel.SetActive(false);
    }
}
