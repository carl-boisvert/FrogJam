using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioDataController : MonoBehaviour
{
    public MusicType _musicPlaying = MusicType.None;

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
    }
}
