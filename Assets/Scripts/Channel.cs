using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    [SerializeField] private MusicType _type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RadioAiguille")
        {
            GameEvents.OnEnterMusicChannel(_type);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RadioAiguille")
        {
            GameEvents.OnExitMusicChannel();
        }
    }
}
