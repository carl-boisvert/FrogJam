using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePressStart : MonoBehaviour
{
    [SerializeField] private MainMenuController _controller;
    
    private void Hide()
    {
        gameObject.SetActive(false);
        _controller.ShowButtons();
    }
}
