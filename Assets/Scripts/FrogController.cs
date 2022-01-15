using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FrogController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _radio;

    // Start is called before the first frame update

    private void Start()
    {
        GameEvents.OnDayEndEvent += OnDayEndEvent;
    }

    private void OnDayEndEvent(int day, int score, bool islastday)
    {
        Destroy(gameObject);
    }

    public void Init(GameObject radio)
    {
        _radio = radio;
        _anim.SetTrigger("Jump");
    }

    private void Update()
    {
        if (_radio != null && _agent.enabled)
        {
            _agent.destination = _radio.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Radio")
        {
            RadioDataController radioDataController = other.gameObject.GetComponent<RadioDataController>();
            radioDataController.PlayFrogMusic();
            _agent.isStopped = true;
            _anim.SetTrigger("Stop");
        }
    }

    public void PickedUp()
    {
        _agent.enabled = false;
        _anim.SetTrigger("Stop");
    }
}
