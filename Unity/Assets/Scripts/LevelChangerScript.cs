using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelChangerScript : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    bool AlreadyPlayed = false;
    //bool stop = false;
    private int _levelToLoad;
    
    void Start()
    {
        StartCoroutine("StartLevel");
    }

    public IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Fading in...");
        animator.SetTrigger("FadeIn");
    }

    public void FadeToLevel (int levelIndex)
    {
        _levelToLoad = levelIndex;
        if(levelIndex == 2) animator.SetTrigger("FadeOut_Fast");
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
            audioSource.Play();
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
