using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodexFlip : MonoBehaviour
{
    private Animator _agendaAnimator;

    public Texture[] _totalPages;

    public List<Texture> _discoveredPagesList;

    public Texture[] _discoveredPages;

    public Texture _backgroundTexture;

    [SerializeField] private Codex codex;


    void Start()
    {
        _agendaAnimator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        if (codex._discoveredIndex.Count > 0) UpdateDiscovered(codex._discoveredIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            FlipLToR();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            FlipRToL();
    }

    public void FlipRToL()
    {
        if (_agendaAnimator == null || codex._currentPage == _discoveredPagesList.Count - 1 || _discoveredPagesList.Count == 0)
            return;

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
            return;

        _agendaAnimator.SetBool(name: "FlipLToR", true);

    }

    public void ResetRPage()
    {

        _agendaAnimator.SetBool(name: "FlipRToL", false);

        setTexture("Left", _discoveredPagesList[codex._currentPage]);

        codex._currentPage++;

        if (codex._currentPage == _discoveredPagesList.Count) setTexture("Right", _backgroundTexture);
        else setTexture("Right", _discoveredPagesList[codex._currentPage]);

    }

    public void setNextLeftBG()
    {
        setTexture("Left_BG", _discoveredPagesList[codex._currentPage]);
    }

    public void setPreviousLeftBG()
    {
        if (codex._currentPage - 1 == 0) setTexture("Left_BG", _backgroundTexture);
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

        if (codex._currentPage > indexes.Count - 1)
        {
            setTexture("Right", _backgroundTexture);
            setTexture("Right_BG", _backgroundTexture);
        }
        else
        {
            setTexture("Right", _discoveredPagesList[codex._currentPage]);
            setTexture("Right_BG", _discoveredPagesList[codex._currentPage]);
        }

        if (codex._currentPage == 0)
        {
            setTexture("Left_BG", _backgroundTexture);
            setTexture("Left", _backgroundTexture);
        }
        else
        {
            setTexture("Left_BG", _discoveredPagesList[codex._currentPage - 1]);
            setTexture("Left", _discoveredPagesList[codex._currentPage - 1]);
        }

    }
}
