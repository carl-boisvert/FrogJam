using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawnController : MonoBehaviour
{
    [SerializeField] private GameObject _customerPrefabs;
    [SerializeField] private FileController _fileController;
    void Start()
    {
        GameEventSystem.OnNewOrderEvent += OnNewOrderEvent;   
    }

    private void OnNewOrderEvent(Order order)
    {
        Transform filePosition = _fileController.getNextAvailablePosition();
        GameObject go = Instantiate(_customerPrefabs, transform.position, transform.rotation);
        CustomerController ctrl = go.GetComponent<CustomerController>();
        ctrl.SetFilePosition(filePosition);
    }

    private void OnDestroy()
    {
        GameEventSystem.OnNewOrderEvent -= OnNewOrderEvent;
    }
}
