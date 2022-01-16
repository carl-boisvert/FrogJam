using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioController : MonoBehaviour
{
    [SerializeField] private GameObject _aiguille;
    [SerializeField] private float _aiguilleSpeed;
    [SerializeField] private Camera _camera;
    [SerializeField] private AudioSource _speaker;
    [SerializeField] private GameObject _tooltip;

    private MusicType _musicTypePlaying;
    private RadioControls _radioControls;
    private InputAction _aiguilleInput;
    private InputAction _escapeInput;
    // Start is called before the first frame update
    void Start()
    {
        _radioControls = new RadioControls();
        _aiguilleInput = _radioControls.Radio.Aiguille;
        _escapeInput = _radioControls.Radio.Escape;
        
        GameEvents.OnLookAtRadioEvent += OnLookAtRadioEvent;
    }



    private void OnLookAtRadioEvent()
    {
        _camera.enabled = true;
        if (_aiguilleInput != null)
        {
            _aiguilleInput.Enable();
            _escapeInput.Enable();
            _tooltip.SetActive(true);
        }
    }

    private void Update()
    {
        if (_aiguilleInput.inProgress)
        {
            _aiguille.transform.position += new Vector3(_aiguilleInput.ReadValue<float>(), 0 ,0) * _aiguilleSpeed * Time.deltaTime;
        }

        if (_escapeInput.triggered)
        {
            _camera.enabled = false;
            _aiguilleInput.Disable();
            _escapeInput.Disable();
            _tooltip.SetActive(false);
            GameEvents.OnStopLookAtRadioEvent();
        }
    }
}

[Serializable]
public class MusicTypeAudioClip
{
    public MusicType _type;
    public AudioClip _audio;
}
