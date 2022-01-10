using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUIController : MonoBehaviour
{
    [SerializeField] private Image iconSlot;
    [SerializeField] private OrderTimer timer;


    public void Init(Order order, Sprite plantIcon, float orderTime)
    {
        iconSlot.sprite = plantIcon;
        timer._duration = orderTime;
        timer.order = order;
    }
}
