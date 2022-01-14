using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioDataController : MonoBehaviour
{
    [SerializeField] private List<MusicTypeAudioClip> _musicTypeAudioClips;
    [SerializeField] private AudioClip _whiteNoise;
    [SerializeField] private AudioSource _speaker;
    
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

    private void OnStopLookAtRadioEvent(MusicType musicTypePlaying)
    {
        _musicPlaying = musicTypePlaying;
        if (radioSpot != null)
        {
            radioSpot.MusicPlaying(musicTypePlaying);
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
