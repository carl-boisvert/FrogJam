using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumbotronController : MonoBehaviour
{

    [SerializeField] private GameObject _orderImagePrefab;
    [SerializeField] private GameObject _screen;
    [SerializeField] private Dictionary<Order, GameObject> _ordersGameObjects = new Dictionary<Order, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.OnNewOrderEvent += OnNewOrderEvent;
        GameEvents.OnOrderDoneEvent += OnOrderDoneEvent;
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
        go.GetComponent<Image>().color = order.plants[0].color;
        _ordersGameObjects.Add(order, go);
    }

    private void OnDestroy()
    {
        GameEvents.OnNewOrderEvent -= OnNewOrderEvent;
    }
}
