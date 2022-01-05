using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    [SerializeField] private List<Order> _orders;

    private void Update()
    {
        Debug.Log(_orders);
    }
}
