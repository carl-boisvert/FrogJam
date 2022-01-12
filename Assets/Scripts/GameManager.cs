using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private OrdersData _ordersData;
    [SerializeField] private List<Order> _orders;
    [SerializeField] private GameObject _frogPrefab;
    [SerializeField] private List<GameObject> _frogSpawnPoint;
    [SerializeField] private float _frogTimer;
    [SerializeField] private GameObject _radio;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _hapinnessStart = 50;
    [SerializeField] private JumbotronController _jumbotronController;

    private float _nextOrderTime;
    private int _currentStage = 0;
    
    private Coroutine _coroutine;

    private void Start()
    {
        Invoke("NewPhase",5);
        Invoke("StartSpawningFrog",5);
        
        GameEvents.OnOrderTimerExpiredEvent += OnOrderTimerExpiredEvent;
        
        _jumbotronController.SetHappiness(_hapinnessStart);
    }

    private void OnOrderTimerExpiredEvent(Order order)
    {
        _orders.Remove(order);
        if (_score + _ordersData.stages[_currentStage].pointPerOrderExpired > 0)
        {
            _score += _ordersData.stages[_currentStage].pointPerOrderExpired;
            _jumbotronController.SetScore(_score);   
        }
    }

    private void NewPhase()
    {
        if (_currentStage < _ordersData.stages.Count)
        {
            _coroutine = StartCoroutine(CreateNextOrder(_ordersData.stages[_currentStage]));
        }
    }

    private void StartSpawningFrog()
    {
        StartCoroutine(SpawnFrog());
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
            Order order = CreateOrder(stage);

            GameEvents.OnNewOrderEvent(order);
        
            Debug.Log($"New Order in! Next order in {nextOrder}");
            yield return new WaitForSeconds(nextOrder);
        
            time = Time.time;
            nextOrder = Random.Range(stage.timeBetweenOrdersMin, stage.timeBetweenOrdersMax);
        }
        Debug.Log($"End of order phase {_currentStage} at {time}");
        _currentStage++;
        NewPhase();
    }
    
    IEnumerator SpawnFrog()
    {
        yield return new WaitForSeconds(Random.Range(_ordersData.stages[_currentStage].timeBetweenFrogsMin, _ordersData.stages[_currentStage].timeBetweenFrogsMax));
        GameObject frog = Instantiate(_frogPrefab, _frogSpawnPoint[Random.Range(0, _frogSpawnPoint.Count)].transform);
        FrogController frogController = frog.GetComponent<FrogController>();
        frogController.Init(_radio);
        StartSpawningFrog();
    }

    private Order CreateOrder(OrderPlantDataStage stage)
    {
        Order order = new Order();
        //Select a plant randomly;
        order.plants = new List<PlantData>();
        for (int i = 0; i < stage.maximumOfPlantPerOrder; i++)
        {
            order.plants.Add(stage.plantThatCanSpawn[Random.Range(0,stage.plantThatCanSpawn.Count )]);
        }

        order.time = stage.timePerOrder;
        _orders.Add(order);
        return order;
    }

    public void SellPlant(List<PlantController> plantsInHand)
    {
        //PlantData data = plantInHand.GetPlantData();
        Order soldOrder = null;
        foreach (var order in _orders)
        {
            if (order.plants.Count == plantsInHand.Count)
            {
                bool orderIsDone = true;
                foreach (var orderPlant in order.plants)
                {
                    if (plantsInHand.Find(plant => plant.GetPlantData().name == orderPlant.name) == null)
                    {
                        orderIsDone = false;
                    }
                }

                foreach (var plantController in plantsInHand)
                {
                    if (plantController.isDead)
                    {
                        orderIsDone = false;
                    }
                }

                if (orderIsDone)
                {
                    soldOrder = order;
                    foreach (var plantController in plantsInHand)
                    {
                        Destroy(plantController.gameObject);
                    }
                    _orders.Remove(soldOrder);
                    _score += _ordersData.stages[_currentStage].pointPerOrderDone;
                    _jumbotronController.SetScore(_score);
                    _jumbotronController.SetHappiness(_score);
                    GameEvents.OnOrderDoneEvent(soldOrder);
                    break;
                }

            }
        }
    }
}
