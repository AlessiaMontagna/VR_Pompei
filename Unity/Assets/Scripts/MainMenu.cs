using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class MainMenu : MonoBehaviour
{
    public MouseLook _mouse;
    public GameObject mainMenuHolder_it;
    public GameObject mainMenuHolder_en;

    public GameObject optionsMenuHolder_it;
    public GameObject optionsMenuHolder_en;

    public GameObject controlsMenuHolder_it;
    public GameObject controlsMenuHolder_en;

    public GameObject languageSelectHolder;
    public GameObject inputController;
    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public Toggle[] resolutionToggles_en;
    public Toggle fullscreenToggle;
    public Toggle fullscreenToggle_en;
    public int[] screenWidths;
    int activeScreenResIndex;

    public void Start()
    {
        _mouse.SetCursorLock(false);
        activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
        bool isFullscreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;

        if (String.Compare(Globals.language, "it") == 0)  
        {  
            for (int i = 0; i<resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = i == activeScreenResIndex;
        }
            fullscreenToggle.isOn = isFullscreen;
        }
        if (String.Compare(Globals.language, "en") == 0)
            {
                for(int i = 0; i < resolutionToggles_en.Length;i++)
            {
                resolutionToggles_en[i].isOn = i == activeScreenResIndex;
            }
                
                    fullscreenToggle_en.isOn = isFullscreen;
                
            }
    }
    public void PlayGame()
    {
        Globals.someoneIsTalking = false;
        Globals.earthquake = false;
        Globals.gamePause = false;


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
        if (String.Compare(Globals.language, "it") == 0)
        {
            if (resolutionToggles[i].isOn)
            {
                activeScreenResIndex = i;
                float aspectRatio = 16 / 9f;
                Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
                PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
                PlayerPrefs.Save();
            }
            for (int j = 0; j < resolutionToggles.Length; j++)
            {
                if (j != i)
                {
                    resolutionToggles[j].isOn = false;
                }
            }
        }
        else if (String.Compare(Globals.language,"en") == 0)
        {
            if (resolutionToggles_en[i].isOn)
            {
                activeScreenResIndex = i;
                float aspectRatio = 16 / 9f;
                Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
                PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
                PlayerPrefs.Save();
            }
            for (int j = 0; j < resolutionToggles_en.Length; j++)
            {
                if (j != i)
                {
                    resolutionToggles_en[j].isOn = false;
                }
            }
        }
    }

    public void SetLanguage(string lang)
    {
        Globals.language = lang;
    }

    public void SetMasterVolume(float value)
    {
        //Debug.Log("master volume set to " + value);
        Globals.masterVolume = value;
    }


    public void SetVoiceVolume(float value)
    {
        //Debug.Log("voice volume set to " + value);
        Globals.talkVolume = value;
    }


    public void SetSFXVolume(float value)
    {   
        //Debug.Log("SFX volume set to " + value);
        Globals.sfxVolume = value;
    }


    public void SetFullScreen(bool isFullScreen)
    {
        if (String.Compare(Globals.language, "it") == 0)
        {
            for (int i = 0; i < resolutionToggles.Length; i++)
            {
                resolutionToggles[i].interactable = !isFullScreen;
                resolutionToggles[i].isOn = false;
            }

            if (isFullScreen)
            {
                Resolution[] allResolutions = Screen.resolutions;
                Resolution maxResolution = allResolutions[allResolutions.Length - 1];
                Screen.SetResolution(maxResolution.width, maxResolution.height, true);
            }
            else
            {
                SetScreenResolution(activeScreenResIndex);
            }
            PlayerPrefs.SetInt("fullscreen", ((isFullScreen) ? 1 : 0));
        }

        else if (String.Compare(Globals.language, "en") == 0)
        {
            for (int i = 0; i < resolutionToggles_en.Length; i++)
            {
                resolutionToggles_en[i].interactable = !isFullScreen;
                resolutionToggles[i].isOn = false;
            }

            if (isFullScreen)
            {
                Resolution[] allResolutions = Screen.resolutions;
                Resolution maxResolution = allResolutions[allResolutions.Length - 1];
                Screen.SetResolution(maxResolution.width, maxResolution.height, true);
            }
            else
            {
                SetScreenResolution(activeScreenResIndex);
            }
            PlayerPrefs.SetInt("fullscreen", ((isFullScreen) ? 1 : 0));
        }

    }

    public void SetInputController(string input)
    {
        Globals.input = input;
    }

    public void LanguageMenu()
    {
        languageSelectHolder.SetActive(true);
        inputController.SetActive(false);
    }
}
