using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioDataController : MonoBehaviour
{
    [SerializeField] private List<MusicTypeAudioClip> _musicTypeAudioClips;
    [SerializeField] private AudioClip _whiteNoise;
    [SerializeField] private AudioSource _speaker;
    [SerializeField] private Transform _startingTransform;
    
    public MusicType _musicPlaying = MusicType.None;
    [SerializeField] private List<ParticleSystem> _particleMusicsPlaying;
    

    public RadioSpot radioSpot;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.OnStopLookAtRadioEvent += OnStopLookAtRadioEvent;
        GameEvents.OnLookAtRadioEvent += OnLookAtRadioEvent;
        GameEvents.OnEnterMusicChannel += OnEnterMusicChannel;
        GameEvents.OnExitMusicChannel += OnExitMusicChannel;
        GameEvents.OnFrogChangedMusic += OnFrogChangedMusic;
        GameEvents.OnDayEndEvent += OnDayEndEvent;
        GameEvents.OnGameEndEvent += OnGameEndEvent;
    }

    private void OnGameEndEvent()
    {
        _speaker.Stop();
        foreach (var system in _particleMusicsPlaying)
        {
            system.Stop();
        }

        ResetPosition();
    }

    private void OnDayEndEvent(int day, int score, bool islastday)
    {
        _speaker.Stop();
        foreach (var system in _particleMusicsPlaying)
        {
            system.Stop();
        }

        ResetPosition();
    }
    
    public void ResetPosition()
    {
        gameObject.transform.position = _startingTransform.position;
        gameObject.transform.rotation = _startingTransform.rotation;
    }

    private void OnLookAtRadioEvent()
    {
        if (!_speaker.isPlaying)
        {
            _musicPlaying = MusicType.None;
            _speaker.clip = _whiteNoise;
            _speaker.Play();
        }
    }

    private void OnStopLookAtRadioEvent()
    {
        if (radioSpot != null)
        {
            radioSpot.MusicPlaying(_musicPlaying);
        }
        
        if (_musicPlaying != MusicType.None)
        {
            if (_particleMusicsPlaying[0].isStopped)
            {
                foreach (var system in _particleMusicsPlaying)
                {
                    system.Play();
                }
            }
        }
        else
        {
            _speaker.Stop();
            foreach (var system in _particleMusicsPlaying)
            {
                system.Stop();
            }
        }
    }

    public void PlayFrogMusic()
    {
        GameEvents.OnFrogChangedMusic();
    }
    
    private void OnFrogChangedMusic()
    {
        _musicPlaying = MusicType.Frog;
        AudioClip clip = _musicTypeAudioClips.Find(musicType => musicType._type == MusicType.Frog)._audio;
        _speaker.clip = clip;
        _speaker.Play();
    }

    private void OnExitMusicChannel()
    {
        if (_musicPlaying == MusicType.Frog)
        {
            GameEvents.OnStoppedFrogMusic();
        }

        _musicPlaying = MusicType.None;
        _speaker.clip = _whiteNoise;
        _speaker.Play();
    }

    private void OnEnterMusicChannel(MusicType type)
    {
        if (_musicPlaying == MusicType.Frog)
        {
            GameEvents.OnStoppedFrogMusic();
        }
        _musicPlaying = type;
        AudioClip clip = _musicTypeAudioClips.Find(musicType => musicType._type == type)._audio;
        _speaker.clip = clip;
        _speaker.Play();
    }
}
