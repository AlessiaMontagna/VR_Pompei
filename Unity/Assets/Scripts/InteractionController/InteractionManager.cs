using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Transform _fpsCameraT;
    [SerializeField] private float _interactionDistance;
    private Interattivo _pointingInteractable;
    private ObjectInteractable _objectInteractable;
    private CharacterController _fpsController;
    private Vector3 _rayOrigin;


    void Start()
    {
        if (_fpsCameraT == null)
        {
            _fpsCameraT = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        _fpsController = GetComponent<CharacterController>();
        _interactionDistance = 3f;
    }

    void Update()
    {
        // Creo ogni volta il raggio, dalla posizione della camera a una distanza pari a un raggio lungo la forward. 
        _rayOrigin = _fpsCameraT.position + _fpsController.radius * _fpsCameraT.forward;
        Ray ray = new Ray(_rayOrigin, _fpsCameraT.forward);
        CheckInteraction(ray);
        CheckObjectInteraction(ray);

    }
    private void CheckObjectInteraction(Ray ray)
    {
        if (_objectInteractable != null)
        {
            _objectInteractable.UITextOff();
        }
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            //Check if is interactable
            _objectInteractable = hit.transform.GetComponent<ObjectInteractable>(); //recupero un oggetto di tipo interactable.
            if (_objectInteractable)
            {
                _objectInteractable.UITextOn();
                if (CrossPlatformInputManager.GetButtonDown("Interact"))
                    _objectInteractable.Interact();
            }
        }
        else
        {
            _objectInteractable = null;
        }
    }
    //Creo un raggio a partire dall'origine. Devo sapere se l'oggetto che sto puntando è interagibile e con esso faccio partire un tipo di interazione.
    private void CheckInteraction(Ray ray)
    {
        if (_pointingInteractable != null)
        {
            _pointingInteractable.UITextOff();
            _pointingInteractable = null;
        }
        if (Globals.someoneIsTalking) return;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            //Check if is interactable
            _pointingInteractable = hit.transform.GetComponent<Interattivo>(); //recupero un oggetto di tipo interactable.
            if (_pointingInteractable) //se non è nullo, invoco la funzione interact
            {
                float angleToTarget = Vector3.Angle(_pointingInteractable.transform.forward, (transform.position - _pointingInteractable.transform.position).normalized);
                if (angleToTarget < 45f)
                {
                    _pointingInteractable.UITextOn();
                    if (CrossPlatformInputManager.GetButtonDown("Interact"))
                        _pointingInteractable.Interact(gameObject);
                }
            }
        }
        //If NOTHING is detected set all to null
        else
        {
            _pointingInteractable = null;
        }
    }

}
