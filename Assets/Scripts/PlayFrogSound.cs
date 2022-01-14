using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayFrogSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _frogSound;
    [SerializeField] private float _playPercentage;

    private void Start()
    {
        _audioSource.PlayOneShot(_frogSound);
    }

    public void PlayFrogAudio()
    {
        float roll = Random.Range(0, 100);
        if (roll <= _playPercentage)
        {
            _audioSource.PlayOneShot(_frogSound);
        }
    }
}
