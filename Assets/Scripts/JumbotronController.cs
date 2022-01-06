using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumbotronController : MonoBehaviour
{

    [SerializeField] private GameObject _orderImagePrefab;
    [SerializeField] private GameObject _screen;
    // Start is called before the first frame update
    void Start()
    {
        GameEventSystem.OnNewOrderEvent += OnNewOrderEvent;
    }

    private void OnNewOrderEvent(Order order)
    {
        Instantiate(_orderImagePrefab, _screen.transform);
    }

    private void OnDestroy()
    {
        GameEventSystem.OnNewOrderEvent -= OnNewOrderEvent;
    }
}
