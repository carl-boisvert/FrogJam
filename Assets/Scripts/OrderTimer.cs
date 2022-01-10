using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTimer : MonoBehaviour
{

    [SerializeField] private Image timerLeft;

    public float _duration;
    public Order order;
    private float _currentTime = 0;
    private bool _orderExpired = false;
    
    

    // Update is called once per frame
    void Update()
    {
        if (!_orderExpired)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime < _duration)
            {
                timerLeft.fillAmount = (_duration-_currentTime) / _duration;
            }
            else
            {
                Debug.Log("Order expired");
                _orderExpired = true;
                GameEvents.OnOrderTimerExpiredEvent(order);
            }
        }
    }
}
