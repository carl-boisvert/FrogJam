using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMainMenuButtons : MonoBehaviour
{
    [SerializeField] private MainMenuController _controller;
    
    private void Hide()
    {
        _controller.StartGame();
    }
}
