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
    public float timePerOrderMin;
    public float timePerOrderMax;
    public int pointPerOrderDone;
    public int pointPerOrderExpired;
    public float timeBetweenOrdersMin;
    public float timeBetweenOrdersMax;
    public List<PlantData> plantThatCanSpawn;
    public int maximumOfPlantPerOrder;
    public bool hasColorModifier = false;
    public float timeBetweenFrogsMin;
    public float timeBetweenFrogsMax;
}
