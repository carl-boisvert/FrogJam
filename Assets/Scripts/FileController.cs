using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FileController : MonoBehaviour
{
    [SerializeField] private int _numberOfCustomer = 0;

    [SerializeField] private List<Transform> _positions;

    private void Start()
    {
        _positions = GetComponentsInChildren<Transform>().ToList();
        _positions.RemoveAt(0);
    }

    // Start is called before the first frame update
    public Transform getNextAvailablePosition()
    {
        Transform t = _positions[_numberOfCustomer];
        _numberOfCustomer++;
        return t;
    }
}
