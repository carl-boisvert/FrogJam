using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _pressStartCanvas;
    [SerializeField] private GameObject _controlsCanvas;
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _controlButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Animator _pressStartAnim;
    [SerializeField] private Animator _buttonAnim;

    private MainMenuControl _menuControl;
    private InputAction _pressStartInputAction;

    private void Start()
    {
        _menuControl = new MainMenuControl();
        
        _pressStartInputAction = _menuControl.Menu.PressStart;
        _pressStartInputAction.Enable();
        
        _startButton.onClick.AddListener(OnStartPressed);
    }

    public void StartGame()
    {
        HideMenu();
        GameEvents.OnGameStartEvent();
    }

    public void OnStartPressed()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _buttonAnim.SetTrigger("SlideOut");
    }

    private void Update()
    {
        if (_pressStartInputAction.triggered)
        {
            _pressStartAnim.SetTrigger("SlideOut");
        }
    }

    public void ShowButtons()
    {
        _menuCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ResetMenu()
    {
        throw new NotImplementedException();
    }

    public void HideMenu()
    {
        _pressStartCanvas.SetActive(false);
        _controlsCanvas.SetActive(false);
        _menuCanvas.SetActive(false);
    }
}
