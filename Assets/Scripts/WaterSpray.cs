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
    [SerializeField] private Animator _sprayAnim;
    [SerializeField] private Transform _startingTransform;

    private void Start()
    {
        GameEvents.OnDayEndEvent += OnDayEndEvent;
        GameEvents.OnGameEndEvent += OnGameEndEvent ;
    }

    private void OnGameEndEvent()
    {
        ResetPosition();
    }

    private void OnDayEndEvent(int day, int score, bool islastday)
    {
        ResetPosition();
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _startingTransform.position;
        gameObject.transform.rotation = _startingTransform.rotation;
    }

    public void Refill()
    {
        _waterLevel = _maxWaterLevel;
    }

    public void Spray()
    {
        if (_waterLevel > 0)
        {
            _waterLevel--;
            _sprayAnim.SetTrigger("Spray");
            _particle.Play();
            _audioSource.PlayOneShot(_sprayAudioClip);
        }
    }
}
