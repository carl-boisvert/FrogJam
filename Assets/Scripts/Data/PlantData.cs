using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "plant", menuName ="Data/plant" )]
public class PlantData : ScriptableObject
{
    public string name;
    public Texture2D icon;
    public Color color;
    public bool needWater;
    public bool needLight;
    public List<PlantDataStage> stages;
    public float musicLikeMultiplier;
    public float musicDislikeTime;
    public List<MusicType> musicLikes;
    public List<MusicType> musicDisikes;
}
[Serializable]
public enum MusicType
{
    Jazz,
    Classical,
    Country,
    Rock,
    HipHop
}

[Serializable]
public class PlantDataStage
{
    public float time;
    public GameObject prefab;
}
