using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Order
{
    [SerializeField] public List<PlantData> plants;
    [SerializeField] public List<PlantColor> colors;
    [SerializeField] public float time;
}
