using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject controlsMenuHolder;

    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public int[] screenWidths;
    int activeScreenResIndex;

  public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
  public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
   public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
        controlsMenuHolder.SetActive(false);
    }
    public void MainsMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
        controlsMenuHolder.SetActive(false);
    }
    public void ControlsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(false);
        controlsMenuHolder.SetActive(true);
    }

    public void SetScreenResolution(int i)
    {
        if(resolutionToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i]/aspectRatio), false);
        }
    }
    public void SetFullScreen(bool isFullScreen)
    {
        for(int i =0; i< resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullScreen;
           
        }

        if(isFullScreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResIndex);
        }
    }
}
