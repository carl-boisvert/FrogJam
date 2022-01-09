using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSpot : MonoBehaviour
{
    [SerializeField] private List<GardenSlot> _gardenSlotsLinked;

    public void MusicPlaying(MusicType musicType)
    {
        foreach (var gardenSlot in _gardenSlotsLinked)
        {
            gardenSlot.MusicPlaying(musicType);
        }

        
    }
    
}
