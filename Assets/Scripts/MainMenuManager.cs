using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject quitGamePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text volumeTxt = null;
    [SerializeField] private AudioMixer audioMixer = null;
    [SerializeField] private TMP_Dropdown resDropdown;
    Resolution[] resolutions;

    private void Start()
    {
        quitGamePanel.SetActive(false);
        settingsPanel.SetActive(false);

        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();

        List<string> resOptions = new List<string>();

        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            resOptions.Add(option);

            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
            {
                currentResIndex = i;
            }
        }

        resDropdown.AddOptions(resOptions);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
    }

    public void LoadScene(int index) 
    {
        SceneManager.LoadScene(index);
    }

    public void OpenQuitPanel()
    {
        ClosePanels();
        quitGamePanel.SetActive(true);
    }
    
    public void OpenSettingsPanel()
    {
        ClosePanels();
        settingsPanel.SetActive(true);
    }

    public void Quit() 
    {
        Application.Quit();
    }

    void ClosePanels()
    {
        buttonsPanel.SetActive(false);
        quitGamePanel.SetActive(false );
        settingsPanel.SetActive(false);
    }

    public void VolumeSlider(float volume)
    {
        volume = volumeSlider.value;
        if(volumeSlider.value > -20)
        {
            volumeTxt.text = (volume + 80 + 20).ToString("0");
        }
        else
        {
            volumeTxt.text = (volume + 80 ).ToString("0");
        }
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
