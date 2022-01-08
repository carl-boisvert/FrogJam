using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioController : MonoBehaviour
{

    [SerializeField] private List<MusicTypeAudioClip> _musicTypeAudioClips;
    [SerializeField] private AudioClip _whiteNoise;
    [SerializeField] private GameObject _aiguille;
    [SerializeField] private float _aiguilleSpeed;
    [SerializeField] private Camera _camera;
    [SerializeField] private AudioSource _speaker;

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
        GameEvents.OnEnterMusicChannel += OnEnterMusicChannel;
        GameEvents.OnExitMusicChannel += OnExitMusicChannel;
    }

    private void OnExitMusicChannel()
    {
        _speaker.clip = _whiteNoise;
        _speaker.Play();
    }

    private void OnEnterMusicChannel(MusicType type)
    {
        AudioClip clip = _musicTypeAudioClips.Find(musicType => musicType._type == type)._audio;
        _speaker.clip = clip;
        _speaker.Play();
    }

    private void OnLookAtRadioEvent()
    {
        if (!_speaker.isPlaying)
        {
            _speaker.clip = _whiteNoise;
            _speaker.Play();
        }

        _camera.enabled = true;
        if (_aiguilleInput != null)
        {
            _aiguilleInput.Enable();
            _escapeInput.Enable();
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
