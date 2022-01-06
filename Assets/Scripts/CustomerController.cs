using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{

    [SerializeField] private NavMeshAgent _agent;
    
    private Transform _target;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_target.position);
    }

    public void SetFilePosition(Transform target)
    {
        _target = target;
    }
}
