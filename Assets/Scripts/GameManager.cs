using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private OrdersData _ordersData;
    [SerializeField] private List<Order> _orders;

    private float _nextOrderTime;
    private int _currentStage = 0;
    
    private Coroutine _coroutine;

    private void Start()
    {
        NewPhase();
    }

    private void NewPhase()
    {
        if (_currentStage < _ordersData.stages.Count - 1)
        {
            _coroutine = StartCoroutine(CreateNextOrder(_ordersData.stages[_currentStage]));
        }
    }

    IEnumerator CreateNextOrder(OrderPlantDataStage stage)
    {
        float time = Time.time;
        float stageTime = time + stage.stageDuration;
        float nextOrder = Random.Range(stage.timeBetweenOrdersMin, stage.timeBetweenOrdersMax);
        Debug.Log($"Start of order phase {_currentStage} at {time} and stopping at {stageTime}");
        while (time < stageTime)
        {
            if (time + nextOrder > stageTime)
            {
                nextOrder = stageTime - time;
            }
            Order order = new Order();
            //Select a plant randomly;
            order.plants = stage.plantThatCanSpawn;
            _orders.Add(order);

            GameEventSystem.OnNewOrderEvent(order);
            
            Debug.Log($"New Order in! Next order in {nextOrder}");
            yield return new WaitForSeconds(nextOrder);
            
            time = Time.time;
            nextOrder = Random.Range(stage.timeBetweenOrdersMin, stage.timeBetweenOrdersMax);
        }
        Debug.Log($"End of order phase {_currentStage} at {time}");
        _currentStage++;
        NewPhase();
    }
}
