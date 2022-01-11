using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiosqueController : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _pointsPrefab;
    [SerializeField] private Transform _pointsSpawnpoint;

    private void Start()
    {
        GameEvents.OnOrderDoneEvent += OnOrderDoneEvent;
    }

    private void OnOrderDoneEvent(Order order)
    {
        Instantiate(_pointsPrefab, _pointsSpawnpoint.position, _pointsSpawnpoint.rotation);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Plant")
        {
            SellPlant(other.gameObject.GetComponent<PlantController>());
        }
    }

    private void SellPlant(PlantController plant)
    {
        List<PlantController> plantsToSell = new List<PlantController>(){plant};
        _gameManager.SellPlant(plantsToSell);
    }
}
