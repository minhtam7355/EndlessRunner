using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle fullscreenToggle;

    public void changeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(resolutionDropdown.value);
        PlayerPrefs.SetInt("QualitySettingPreference", resolutionDropdown.value);
    }

    public void changeMasterVolume()
    {
        float volume = masterSlider.value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterSlider.value)*20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void changeMusicVolumn()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void changeSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void fullscreenToggleChange()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("FullscreenPreference", fullscreenToggle.isOn ? 1 : 0);
    }

    public void SaveChange()
    {
        PlayerPrefs.Save();
    }

    public void UnSaveChange()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume") 
            && PlayerPrefs.HasKey("musicVolume") 
            && PlayerPrefs.HasKey("sfxVolume")
            && PlayerPrefs.HasKey("QualitySettingPreference")
            && PlayerPrefs.HasKey("FullscreenPreference"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            resolutionDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
            fullscreenToggle.isOn = PlayerPrefs.GetInt("FullscreenPreference") == 1 ? true : false;
        }
        else
        {
            masterSlider.value = 1;
            musicSlider.value = 1;
            sfxSlider.value = 1;
            resolutionDropdown.value = 2;
            fullscreenToggle.isOn = true;
        }
    }
    
    private void Update()
    {
        PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        PlayerPrefs.SetInt("QualitySettingPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", fullscreenToggle.isOn ? 1 : 0);
    }
}

