using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "orders", menuName ="Data/orders" )]
public class OrdersData : ScriptableObject
{
    public List<OrderPlantDataStage> stages;
}

[Serializable]
public class OrderPlantDataStage
{
    public float stageDuration;
    public float timeBetweenOrdersMin;
    public float timeBetweenOrdersMax;
    public List<PlantData> plantThatCanSpawn;
    public int maximumOfPlantPerOrder;
    public bool hasColorModifier = false;
}