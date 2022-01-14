using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    
    [SerializeField] private CinemachineVirtualCamera _dolly;
    [SerializeField] private CinemachineVirtualCamera _mainMenu;
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private Button _continueButton;

    private float _nextOrderTime;
    private int _currentStage = 0;
    private bool _hasPhaseEnded = false;
    private int _phaseDone = 0;
    
    private Coroutine _coroutine;

    private void Start()
    {
        GameEvents.OnOrderTimerExpiredEvent += OnOrderTimerExpiredEvent;
        GameEvents.OnGameStartEvent += OnGameStartEvent;
        _continueButton.onClick.AddListener(OnclickedContinueButton);
        
        _jumbotronController.IncreaseHappiness(_hapinnessStart);

        //MainMenu();
    }

    private void OnclickedContinueButton()
    {
        Invoke("NewPhase", 10);
        _hasPhaseEnded = false;
        GameEvents.OnGameContinueEvent();
    }

    private void OnGameStartEvent()
    {
        _mainMenuCanvas.SetActive(false);
        Invoke("NewPhase",5);
        Invoke("StartSpawningFrog",5);
    }

    private void Update()
    {
        if (_hasPhaseEnded && _orders.Count == 0)
        {
            GameEvents.OnGameEndEvent();
        }
    }

    private void OnOrderTimerExpiredEvent(Order order)
    {
        _orders.Remove(order);
        if (_score + _ordersData.stages[_currentStage].pointPerOrderExpired > 0)
        {
            _score += _ordersData.stages[_currentStage].pointPerOrderExpired;
            _jumbotronController.SetScore(_score);
        }
        _jumbotronController.DecreaseHappiness(_ordersData.stages[_currentStage].pointPerOrderExpired);
    }

    private void NewPhase()
    {
        _hasPhaseEnded = false;
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
        
            //Debug.Log($"New Order in! Next order in {nextOrder}");
            yield return new WaitForSeconds(nextOrder);
        
            time = Time.time;
            nextOrder = Random.Range(stage.timeBetweenOrdersMin, stage.timeBetweenOrdersMax);
        }
        Debug.Log($"End of order phase {_currentStage} at {time}");
        _currentStage++;
        _phaseDone++;
        if (_phaseDone == 2)
        {
            _hasPhaseEnded = true;
        }
        else
        {
            NewPhase();
        }
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
        order.colors = new List<PlantColor>();
        for (int i = 0; i < stage.maximumOfPlantPerOrder; i++)
        {
            PlantData plant = stage.plantThatCanSpawn[Random.Range(0, stage.plantThatCanSpawn.Count)];
            if (stage.hasColorModifier)
            {
                int roll = Random.Range(0, 100);
                if (roll <= stage.percentageOfChanceToGetColor)
                {
                    int color = Random.Range(0, 4);
                    Array values = Enum.GetValues(typeof(PlantColor));
                    order.colors.Add((PlantColor)values.GetValue(color));
                }
            }

            order.plants.Add(plant);
        }

        order.time = Random.Range(stage.timePerOrderMin, stage.timePerOrderMax);
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

                if (order.colors.Count > 0)
                {
                    foreach (var plantController in plantsInHand)
                    {
                        if (plantController.plantColor != order.colors[0])
                        {
                            orderIsDone = false;
                        }
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
                    _jumbotronController.IncreaseHappiness(_ordersData.stages[_currentStage].pointPerOrderDone);
                    GameEvents.OnOrderDoneEvent(soldOrder);
                    break;
                }

            }
        }
    }
}
