using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public delegate void NewOrderEvent(Order order);
    public static NewOrderEvent OnNewOrderEvent;
    
    public delegate void OrderDoneEvent(Order order);
    public static OrderDoneEvent OnOrderDoneEvent;
}