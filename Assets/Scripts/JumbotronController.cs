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
    [SerializeField] private Dictionary<Order, GameObject> _ordersGameObjects = new Dictionary<Order, GameObject>();
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float _maxHapiness;
    [SerializeField] private Slider _slider;
    // Start is called before the first frame update
    [SerializeField] private Image _fillImage;

    void Start()
    {
        GameEvents.OnNewOrderEvent += OnNewOrderEvent;
        GameEvents.OnOrderDoneEvent += OnOrderDoneEvent;
        GameEvents.OnOrderTimerExpiredEvent += OnOrderTimerExpiredEvent;
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
        orderUIController.Init(order, order.plants[0].icon, order.time);
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

    public void SetHappiness(float score)
    {
        _slider.value = score / _maxHapiness;
    }
}
