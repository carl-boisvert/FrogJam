using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JumbotronController : MonoBehaviour
{
    [SerializeField] private GameObject _orderImagePrefab;
    [SerializeField] private GameObject _screen;
    [SerializeField] private GameObject _ingameUI;
    [SerializeField] private GameObject _phaseSummary;
    [SerializeField] private SummaryController _summaryController;
    [SerializeField] private Dictionary<Order, GameObject> _ordersGameObjects = new Dictionary<Order, GameObject>();
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float _maxHapiness;
    [SerializeField] private Slider _slider;
    // Start is called before the first frame update
    [SerializeField] private Image _fillImage;

    [SerializeField] private float _happiness;

    void Start()
    {
        GameEvents.OnNewOrderEvent += OnNewOrderEvent;
        GameEvents.OnOrderDoneEvent += OnOrderDoneEvent;
        GameEvents.OnOrderTimerExpiredEvent += OnOrderTimerExpiredEvent;
        GameEvents.OnDayEndEvent += OnDayEndEvent;
        GameEvents.OnGameContinueEvent += OnGameContinueEvent;
        _happiness = 0;
    }

    private void OnGameContinueEvent()
    {
        _ingameUI.SetActive(true);
        _phaseSummary.SetActive(false);
    }

    private void OnDayEndEvent(int day, int score, bool isLastDay)
    {
        _ingameUI.SetActive(false);
        _phaseSummary.SetActive(true);
        _summaryController.SetCurrentDay(day, score, _happiness, isLastDay);
    }

    private void OnOrderTimerExpiredEvent(Order order)
    {
        GameObject go;
        _ordersGameObjects.TryGetValue(order, out go);

        if (go != null)
        {
            _ordersGameObjects.Remove(order);
            Destroy(go);
        }
    }

    private void OnOrderDoneEvent(Order order)
    {
        GameObject go;
        _ordersGameObjects.TryGetValue(order, out go);

        if (go != null)
        {
            _ordersGameObjects.Remove(order);
            Destroy(go);
        }
    }

    private void OnNewOrderEvent(Order order)
    {
        GameObject go = Instantiate(_orderImagePrefab, _screen.transform);
        OrderUIController orderUIController = go.GetComponent<OrderUIController>();
        Sprite icon = order.plants[0].icon;
        if (order.colors.Count > 0)
        {
            switch (order.colors[0])
            {
                case PlantColor.Blue:
                    icon = order.plants[0].iconBlue;
                    break;
                case PlantColor.Green:
                    icon = order.plants[0].iconGreen;
                    break;
                case PlantColor.Pink:
                    icon = order.plants[0].iconPink;
                    break;
                case PlantColor.Red:
                    icon = order.plants[0].iconRed;
                    break;
                case PlantColor.Yellow:
                    icon = order.plants[0].iconYellow;
                    break;
            }
        }

        orderUIController.Init(order, icon, order.time);
        _ordersGameObjects.Add(order, go);
    }

    private void OnDestroy()
    {
        GameEvents.OnNewOrderEvent -= OnNewOrderEvent;
    }

    public void SetScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void IncreaseHappiness(float hapiness)
    {
        _happiness += hapiness;
        if (_happiness >= _maxHapiness)
        {
            _happiness = _maxHapiness;
        }
        _slider.value = _happiness / _maxHapiness;
    }
    
    public void DecreaseHappiness(float hapiness)
    {
        _happiness -= hapiness;

        if (_happiness <= 0)
        {
            _ingameUI.SetActive(false);
            _phaseSummary.SetActive(true);
            GameEvents.OnGameEndEvent();
            _happiness = 0;
        }
        _slider.value = _happiness / _maxHapiness;
    }
}
