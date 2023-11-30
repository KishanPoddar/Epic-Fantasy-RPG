using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject pauseMenuPanel;
    private bool isPaused = false;
    private static int scale = 1;

    public void PauseResumeScreen()
    {
        if(!isPaused)
        {
            pauseButton.GetComponent<Image>().sprite = sprites[0];
            Invoke(nameof(ChangeTimeScale), .005f);
            pauseMenuPanel.SetActive(true);
            isPaused = true;
        }
        else
        {
            pauseButton.GetComponent <Image>().sprite = sprites[1];
            Invoke(nameof(ChangeTimeScale), .005f);
            pauseMenuPanel.SetActive(false);
            isPaused = false;
        }
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    void ChangeTimeScale()
    {
        if(scale == 1)
        {
        scale = 0;
        Time.timeScale = scale;
        }
        else
        {
            scale = 1;
            Time.timeScale = scale;
        }
    }
}
