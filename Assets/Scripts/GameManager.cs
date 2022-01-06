using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PlantData> _plants;
    [SerializeField] private float _timeBetweenOrders;

    private float _nextOrderTime;

    private void Update()
    {
        if (Time.time > _nextOrderTime)
        {
            _nextOrderTime = Time.time + _timeBetweenOrders;
            Debug.Log("Spawn new order");
        }
    }
}
