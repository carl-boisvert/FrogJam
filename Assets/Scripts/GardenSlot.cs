using System;
using UnityEngine;

public class GardenSlot : MonoBehaviour
{
    public bool hasSomething = false;
    public MusicType musicTypePlaying = MusicType.None;
    public PlantController plantController;

    private void Start()
    {
        GameEvents.OnFrogChangedMusic += OnFrogChangedMusic;
        GameEvents.OnStoppedFrogMusic += OnStoppedFrogMusic;
        GameEvents.OnDayEndEvent += OnDayEndEvent;
        GameEvents.OnGameEndEvent += OnGameEndEvent;
    }

    private void OnGameEndEvent()
    {
        hasSomething = false;
        MusicPlaying(MusicType.None);
        if (plantController != null)
        {
            Destroy(plantController.gameObject);
        }
        plantController = null;
    }

    private void OnDayEndEvent(int day, int score, bool islastday)
    {
        hasSomething = false;
        MusicPlaying(MusicType.None);
        if (plantController != null)
        {
            Destroy(plantController.gameObject);
        }
        plantController = null;
    }

    private void OnStoppedFrogMusic()
    {
        MusicPlaying(MusicType.None);
    }

    private void OnFrogChangedMusic()
    {
        MusicPlaying(MusicType.Frog);
    }

    public void MusicPlaying(MusicType musicType)
    {
        musicTypePlaying = musicType;
        if (plantController != null)
        {
            plantController._currentMusicType = musicType;
        }
    }

    public void WaterPlant()
    {
        if (hasSomething)
        {
            plantController.Watered();
        }
    }

    private void OnDestroy()
    {
        GameEvents.OnFrogChangedMusic -= OnFrogChangedMusic;
    }
}
