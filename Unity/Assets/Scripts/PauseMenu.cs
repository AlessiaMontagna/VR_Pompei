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
        fps = FindObjectOfType<FirstPersonController>();
    }
    // Update is called once per frame
    void Update()
    { 
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

   public void Resume()
    {
        _ui.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _mouseLook.m_cursorIsLocked = true;
        pauseMenuUI.SetActive(false);
        optionsMenuHolder.SetActive(false);
        controlsMenuHolder.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        fps.enabled = true;
    }

    public void Pause()
    {
        _ui.SetActive(false);
        _mouseLook.m_cursorIsLocked = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        fps.enabled = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        
    }
    
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LightsSetup 1");
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

}
