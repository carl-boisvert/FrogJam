using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
    [SerializeField] private Sprite _button;
    [SerializeField] private string _text;
    [SerializeField] private Transform _player;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private Image _keyImage;
    [SerializeField] private bool _isActive;
    // Start is called before the first frame update
    void Start()
    {
        _textMeshProUGUI.text = _text;
        _keyImage.sprite = _button;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            _textMeshProUGUI.enabled = true;
            _keyImage.enabled = true;
            transform.LookAt(_player.position);
        }
        else
        {
            _textMeshProUGUI.enabled = false;
            _keyImage.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _isActive = false;
        }
    }
}
