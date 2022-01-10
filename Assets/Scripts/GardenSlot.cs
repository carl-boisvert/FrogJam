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

    private void OnParticleCollision(GameObject other)
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
