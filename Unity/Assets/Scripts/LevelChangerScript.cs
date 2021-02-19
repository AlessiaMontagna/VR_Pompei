using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelChangerScript : MonoBehaviour
{
    public Animator animator;
    private AudioSource _audioSource;
    bool AlreadyPlayed = false;
    private int _levelToLoad;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine("StartLevel");
    }

    public IEnumerator StartLevel()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Scena1":Globals.player = Players.Nobile;break;
            case "ScenaLapilli":Globals.player = Players.Mercante;break;
            case "ScenaFinale":Globals.player = Players.Nobile;break;
            default: Debug.Log($"Starting {SceneManager.GetActiveScene().name}");break;
        }
        yield return new WaitForSeconds(1.0f);
        //Debug.Log("Fading in...");
        animator.SetTrigger("FadeIn");
    }

    public void FadeToLevel (int levelIndex)
    {
        _levelToLoad = levelIndex;
        if(levelIndex == 3 || levelIndex == 0) animator.SetTrigger("FadeOut_Fast");
        else animator.SetTrigger("FadeOut");
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnFadeComplete ()
    {
        SceneManager.LoadScene(_levelToLoad);
    }

    public void SwooshIn()
    {
        if(!AlreadyPlayed)
        {
            _audioSource.Play();
            AlreadyPlayed = true;
        }
        animator.SetTrigger("SwooshIn");
    }

    public void SwooshOut()
    {
        animator.SetTrigger("SwooshOut");
        AlreadyPlayed = false;
    }
}
