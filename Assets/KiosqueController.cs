using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiosqueController : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private void OnTriggerEnter(Collider other)
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
