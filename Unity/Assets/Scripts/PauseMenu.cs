using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private MouseLook _mouseLook;
    [SerializeField] private GameObject _ui;



    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public FirstPersonController fps;
    public GameObject optionsMenuHolder;
    public GameObject controlsMenuHolder;
    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public int[] screenWidhts;
    int activeScreenResIndex;


    public void Start()
    {
        _ui = FindObjectOfType<UI>().gameObject;
        fps = FindObjectOfType<FirstPersonController>();
    }
    // Update is called once per frame
    void Update()
    { 
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused == true)
            {
                Resume();
            }
            else
            {
                Pause();

            }
        }
    }

   public void Resume()
    {
        _ui.SetActive(true);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //_mouseLook.m_cursorIsLocked = true;
        _mouseLook.SetCursorLock(true);
        pauseMenuUI.SetActive(false);
        optionsMenuHolder.SetActive(false);
        controlsMenuHolder.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        fps.enabled = true;
        Globals.gamePause = false;
    }

    public void Pause()
    {
        _ui.SetActive(false);
        _mouseLook.SetCursorLock(false);
        //_mouseLook.m_cursorIsLocked = false;
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        fps.enabled = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Globals.gamePause = true;
    }
    
    public void LoadMenu()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void LoadPauseMenu()
    {
        pauseMenuUI.SetActive(true);
        optionsMenuHolder.SetActive(false);
        controlsMenuHolder.SetActive(false);
    }
    public void LoadOption()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuHolder.SetActive(true);
        controlsMenuHolder.SetActive(false);
    }
    public void ControlsMenu()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuHolder.SetActive(false);
        controlsMenuHolder.SetActive(true);
    }
    public void QuitGame()
    {
        Debug.Log("Quit...");
        Application.Quit();
    }
    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidhts[i], (int)(screenWidhts[i] / aspectRatio), false);
        }
    }
    public void SetFullScreen(bool isFullScreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullScreen;

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

}
