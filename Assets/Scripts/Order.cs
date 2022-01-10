using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Order
{
    [SerializeField] public List<PlantData> plants;
    [SerializeField] public float time;
}
