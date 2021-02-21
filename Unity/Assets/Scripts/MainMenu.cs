using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuHolder_it;
    public GameObject mainMenuHolder_en;

    public GameObject optionsMenuHolder_it;
    public GameObject optionsMenuHolder_en;

    public GameObject controlsMenuHolder_it;
    public GameObject controlsMenuHolder_en;

    public GameObject languageSelectHolder;

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
        if(String.Compare(Globals.language, "it") == 0){
            mainMenuHolder_it.SetActive(false);
            optionsMenuHolder_it.SetActive(true);
            controlsMenuHolder_it.SetActive(false);
        } else if (String.Compare(Globals.language, "en") == 0) {
            mainMenuHolder_en.SetActive(false);
            optionsMenuHolder_en.SetActive(true);
            controlsMenuHolder_en.SetActive(false);
        }
        
        languageSelectHolder.SetActive(false);
        
    }
    public void MainsMenu()
    {    
        if(String.Compare(Globals.language, "it") == 0){
            mainMenuHolder_it.SetActive(true);
            optionsMenuHolder_it.SetActive(false);
            controlsMenuHolder_it.SetActive(false);
        } else if (String.Compare(Globals.language, "en") == 0) {
            mainMenuHolder_en.SetActive(true);
            optionsMenuHolder_en.SetActive(false);
            controlsMenuHolder_en.SetActive(false);
        }
        languageSelectHolder.SetActive(false);
       
    }
    public void ControlsMenu()
    {
        if(String.Compare(Globals.language, "it") == 0){
            mainMenuHolder_it.SetActive(false);
            optionsMenuHolder_it.SetActive(false);
            controlsMenuHolder_it.SetActive(true);
        } else if(String.Compare(Globals.language, "en") == 0) {
            mainMenuHolder_en.SetActive(false);
            optionsMenuHolder_en.SetActive(false);
            controlsMenuHolder_en.SetActive(true);
        }
        languageSelectHolder.SetActive(false);
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

    public void SetLanguage(string lang)
    {
        Globals.language = lang;
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
