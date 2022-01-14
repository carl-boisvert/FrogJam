using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioDataController : MonoBehaviour
{
    public MusicType _musicPlaying = MusicType.None;
    [SerializeField] private List<ParticleSystem> _particleMusicsPlaying;
    

    public RadioSpot radioSpot;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.OnStopLookAtRadioEvent += OnStopLookAtRadioEvent;
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
}
