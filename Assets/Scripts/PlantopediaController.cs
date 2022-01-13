using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantopediaController : MonoBehaviour
{
    [SerializeField] private GameObject _plantopedia;

    private BookControl _control;
    private InputAction _exitInput;

    private void Start()
    {
        _control = new BookControl();
        _exitInput = _control.Book.Exit;
        
        _exitInput.performed += ExitInputOnperformed;
    }

    private void ExitInputOnperformed(InputAction.CallbackContext obj)
    {
        HidePlantopedia();
    }


    public void ShowPlantopedia()
    {
        _exitInput.Enable();
        _plantopedia.SetActive(true);
    }
    
    public void HidePlantopedia()
    {
        GameEvents.OnStopLookAtPlantopediaEvent();
        _exitInput.Disable();
        _plantopedia.SetActive(false);
    }
}
