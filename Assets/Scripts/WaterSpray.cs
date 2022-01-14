using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpray : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private int _maxWaterLevel = 3;
    [SerializeField] private int _waterLevel = 0;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _sprayAudioClip;

    public void Refill()
    {
        _waterLevel = _maxWaterLevel;
    }

    public void Spray()
    {
        if (_waterLevel > 0)
        {
            _waterLevel--;
            _particle.Play();
            _audioSource.PlayOneShot(_sprayAudioClip);
        }
    }
}
