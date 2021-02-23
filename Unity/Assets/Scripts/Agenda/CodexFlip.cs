using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodexFlip : MonoBehaviour
{
    private Animator _agendaAnimator;

    public Texture[] _totalPages;

    public List<Texture> _discoveredPagesList;

    public Texture[] _discoveredPages;

    public Texture _backgroundTexture;

    private AudioSource _audioSource;

    [SerializeField] private ShowAgenda _arrowManager;

    [SerializeField] private Codex codex;


    void Start()
    {
        _audioSource = _arrowManager.GetComponent<AudioSource>();
        _agendaAnimator = GetComponent<Animator>();
    }
    void OnEnable()
    {

        if (codex._discoveredIndex.Count > 0)
        {
            UpdateDiscovered(codex._discoveredIndex);

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (codex._currentPage - 1 == 0)
            {
                //_arrowManager.LeftArrow(false);
            }
            else
            {
                //_arrowManager.LeftArrow(true);
            }
            FlipLToR();
            
              
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (codex._currentPage + 1 == _discoveredPagesList.Count)
            {
                //_arrowManager.RightArrow(false);
            }
            else
            {
                //_arrowManager.RightArrow(true);

            }
            FlipRToL();

        }
    }

    public void FlipRToL()
    {
        if (_agendaAnimator == null || codex._currentPage == _discoveredPagesList.Count - 1 || _discoveredPagesList.Count == 0)
            return;
        _audioSource.clip = Resources.Load<AudioClip>("FeedbackSounds/Turn_Pages");
        _audioSource.Play();
        _agendaAnimator.SetBool(name: "FlipRToL", true);

    }

    public void setNextRightBG()
    {
        if (codex._discoveredIndex.Count == codex._currentPage + 1) setTexture("Right_BG", _backgroundTexture);
        else setTexture("Right_BG", _discoveredPagesList[codex._currentPage + 1]);
    }

    private void setTexture(string child, Texture texture)
    {
        //TODO cambiare anche normal map se si fa il testo inciso
        gameObject
                .transform.Find(child)
                .gameObject.GetComponent<Renderer>()
                .materials[1].mainTexture = texture;
    }

    public void FlipLToR()
    {
        if (_agendaAnimator == null || codex._currentPage == 0)
        {
            return;
        }
            
        _audioSource.clip = Resources.Load<AudioClip>("FeedbackSounds/Turn_Pages");
        _audioSource.Play();
        _agendaAnimator.SetBool(name: "FlipLToR", true);

    }

    public void ResetRPage()
    {

        _agendaAnimator.SetBool(name: "FlipRToL", false);

        setTexture("Left", _discoveredPagesList[codex._currentPage]);

        codex._currentPage++;
        _arrowManager.LeftArrow(true);
        if (_discoveredPagesList.Count > 1 && codex._currentPage != _discoveredPagesList.Count - 1)
        {
            _arrowManager.RightArrow(true);
        }
        else
        {
            _arrowManager.RightArrow(false);
        }
        if (codex._currentPage == _discoveredPagesList.Count) setTexture("Right", _backgroundTexture);
        else setTexture("Right", _discoveredPagesList[codex._currentPage]);

    }

    public void setNextLeftBG()
    {
        setTexture("Left_BG", _discoveredPagesList[codex._currentPage]);
    }

    public void setPreviousLeftBG()
    {
        if (codex._currentPage - 1 == 0)
        {
            setTexture("Left_BG", _backgroundTexture);
        }
        else setTexture("Left_BG", _discoveredPagesList[codex._currentPage - 2]);
    }
    public void setPreviousRightBG()
    {
        setTexture("Right_BG", _discoveredPagesList[codex._currentPage - 1]);
    }

    public void ResetLPage()
    {

        _agendaAnimator.SetBool(name: "FlipLToR", false);

        setTexture("Right", _discoveredPagesList[codex._currentPage - 1]);

        codex._currentPage--;
        if (codex._currentPage==0)
        {
            _arrowManager.LeftArrow(false);
        }
        else
        {
            _arrowManager.LeftArrow(true);
        }

        if(_discoveredPagesList.Count > 1)
        {
            _arrowManager.RightArrow(true);
        }
        else
        {
            _arrowManager.RightArrow(false);
        }
        if (codex._currentPage > 0) setTexture("Left", _discoveredPagesList[codex._currentPage - 1]);
        else setTexture("Left", _discoveredPagesList[0]);
    }

    public void UpdateDiscovered(List<int> indexes)
    {
        
        _discoveredPagesList.Clear();

        for (int i = 0; i < indexes.Count; i++)
        {
            _discoveredPagesList.Add(_totalPages[indexes[i]]);
        }
        if (codex._currentPage == 0)
        {
            _arrowManager.LeftArrow(false);
            if (indexes.Count > 1)
            {
                _arrowManager.RightArrow(true);
            }
            else
            {
                _arrowManager.RightArrow(false);
            }
            setTexture("Left_BG", _backgroundTexture);
            setTexture("Left", _backgroundTexture);
        }
        else
        {
            _arrowManager.LeftArrow(true);
            setTexture("Left_BG", _discoveredPagesList[codex._currentPage - 1]);
            setTexture("Left", _discoveredPagesList[codex._currentPage - 1]);
        }

        if (codex._currentPage > indexes.Count - 1)
        {
            _arrowManager.LeftArrow(true);
            _arrowManager.RightArrow(false);
            setTexture("Right", _backgroundTexture);
            setTexture("Right_BG", _backgroundTexture);
        }
        else
        {
            if (codex._currentPage < indexes.Count - 1)
            {
                _arrowManager.RightArrow(true);
            }
            else
            {
                _arrowManager.RightArrow(false);
            }
            setTexture("Right", _discoveredPagesList[codex._currentPage]);
            setTexture("Right_BG", _discoveredPagesList[codex._currentPage]);
        }

        

    }

}
