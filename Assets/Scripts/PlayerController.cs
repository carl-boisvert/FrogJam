using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _fallSpeed;
    [Header("Interaction")]
    [SerializeField] private float _distance;
    [SerializeField] private float _timeBetweenInteraction;
    [SerializeField] private bool _canInteract;
    [SerializeField] private float _throwForce;
    [SerializeField] private Transform _holdSocket;
    [Header("Link")]
    [SerializeField] private CharacterController _cc;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private GameManager _gameManager;
    
    [Header("Plant")]
    [SerializeField] private GameObject _plantPrefab;

    [SerializeField] private bool _hasSomethingInHand;
    [SerializeField] private PlantData _currentSeed;
    [SerializeField] private List<PlantController> _plantsInHand = new List<PlantController>();
    
    private Transform _cameraTransform;
    private MovementControl _moveControl;
    private LookControl _lookControl;
    private InputAction _moveInput;
    private InputAction _interactInput;

    private Vector2 _dir;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        _moveControl = new MovementControl();
        _lookControl = new LookControl();
        
        _moveInput = _moveControl.Player.Move;
        _moveInput.Enable();

        _interactInput = _lookControl.Mouse.Interact;
        _interactInput.Enable();

        _cameraTransform = Camera.main.transform;
        
        GameEvents.OnOrderDoneEvent += OnOrderDoneEvent;
    }

    private void OnOrderDoneEvent(Order order)
    {
        _plantsInHand.Clear();
        _hasSomethingInHand = false;
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        _dir = _moveInput.ReadValue<Vector2>();

        Vector3 movementDir = _cameraTransform.forward * _dir.y + _cameraTransform.right * _dir.x;

        movementDir.y = 0;

        _cc.Move(movementDir * _speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f);
    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, _distance, _interactableLayer))
        {
            if (hit.collider.tag == "GardenSlot")
            {
                if (_interactInput.triggered && _currentSeed != null && _canInteract)
                {
                    GardenSlot gs = hit.collider.gameObject.GetComponent<GardenSlot>();
                    Plant(gs);
                    _canInteract = false;
                    _hasSomethingInHand = false;
                    StartCoroutine(InteractionTimer());
                }
            } else if (hit.collider.tag == "SeedBag")
            {
                if (_interactInput.triggered  && _canInteract)
                {
                    SeedBag bag = hit.collider.gameObject.GetComponent<SeedBag>();
                    _currentSeed = bag.plantData;
                    _hasSomethingInHand = true;
                    _canInteract = false;
                    StartCoroutine(InteractionTimer());
                }
            } else if (hit.collider.tag == "Plant"  && _canInteract)
            {
                if (_interactInput.triggered && !_hasSomethingInHand)
                {
                    GameObject plant = hit.collider.gameObject;
                    PlantController plantController = plant.GetComponent<PlantController>();
                    plantController.PickedUp();
                    Rigidbody _rb = plant.GetComponent<Rigidbody>();
                    _rb.isKinematic = true;
                    plant.transform.parent = _holdSocket;
                    plant.transform.position = _holdSocket.position;
                    plant.transform.localPosition = Vector3.zero;
                    plant.transform.localRotation = _holdSocket.rotation;
                    _plantsInHand.Add(plantController);
                    _hasSomethingInHand = true;
                    _canInteract = false;
                    StartCoroutine(InteractionTimer());
                }
            } else if (hit.collider.tag == "Kiosque"  && _canInteract)
            {
                if (_interactInput.triggered)
                {
                    if (_plantsInHand.Count > 0)
                    {
                        SellPlant();
                    }
                    _canInteract = false;
                    StartCoroutine(InteractionTimer());
                }
            }
        }
        else
        {
            if (_interactInput.triggered && _hasSomethingInHand  && _canInteract)
            {
                //Throw it
                _holdSocket.DetachChildren();
                foreach (var plantController in _plantsInHand)
                {
                    Rigidbody _rb = plantController.GetComponent<Rigidbody>();
                    _rb.isKinematic = false;
                    _rb.AddForce(_cameraTransform.forward*_throwForce, ForceMode.Impulse);
                }
                _canInteract = false;
                StartCoroutine(InteractionTimer());
            }
        }
    }

    private void SellPlant()
    {
        _gameManager.SellPlant(_plantsInHand);
    }

    private void Plant(GardenSlot slot)
    {
        if (!slot.hasSomething)
        {
            GameObject go = Instantiate(_plantPrefab, slot.transform);
            PlantController ctrl = go.GetComponent<PlantController>();
            ctrl.Init(slot, _currentSeed);
            slot.hasSomething = true;
            _currentSeed = null;
        }
    }
    
    IEnumerator InteractionTimer()
    {
        float time = Time.time;
        float nextInteractionTime = time + _timeBetweenInteraction;
        //Debug.Log($"Start Growing phase {_plantStage} at {time} and stopping at {stageTime}");
        while (time < nextInteractionTime)
        {
            yield return new WaitForSeconds(1);
            time = Time.time;
        }

        _canInteract = true;
    }

    private void OnDestroy()
    {
        GameEvents.OnOrderDoneEvent -= OnOrderDoneEvent;
    }
}
