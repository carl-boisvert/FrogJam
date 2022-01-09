using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed;

    [Header("Interaction")] 
    [SerializeField] private GameObject _hud;
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

    [Header("Camera")] 
    [SerializeField] private Camera _camera;

    [Header("State variable")]
    [SerializeField] private bool _hasSomethingInHand;
    [SerializeField] private PlantData _currentSeed;
    [SerializeField] private List<PlantController> _plantsInHand = new List<PlantController>();
    [SerializeField] private bool _hasRadio;
    [SerializeField] private GameObject _radioGO;
    [SerializeField] private bool _inGardenBoxZone;
    [SerializeField] private RadioSpot _radioSpot;
    [SerializeField] private RadioSpot _currentRadioSpot;
    [SerializeField] private bool _canMove = true;
    [SerializeField] private bool _isLookingAtRadio = false;

    private MovementControl _moveControl;
    private LookControl _lookControl;
    private InputAction _moveInput;
    private InputAction _interactInput;
    private InputAction _useInput;

    private Vector2 _dir;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        _moveControl = new MovementControl();
        _lookControl = new LookControl();
        
        _moveInput = _moveControl.Player.Move;
        _moveInput.Enable();

        _useInput = _moveControl.Player.Use;
        _useInput.Enable();

        _interactInput = _lookControl.Mouse.Interact;
        _interactInput.Enable();

        GameEvents.OnOrderDoneEvent += OnOrderDoneEvent;
        GameEvents.OnStopLookAtRadioEvent += OnStopLookAtRadioEvent;
    }

    private void OnOrderDoneEvent(Order order)
    {
        _plantsInHand.Clear();
        _hasSomethingInHand = false;
    }

    private void Update()
    {
        if (_inGardenBoxZone && _useInput.triggered && _hasRadio)
        {
            RadioSpot radioSpot = _radioSpot.GetComponent<RadioSpot>();
            RadioDataController radioDataController = _radioGO.GetComponent<RadioDataController>();
            radioSpot.MusicPlaying(radioDataController._musicPlaying);
            _currentRadioSpot = radioSpot;
            radioDataController.radioSpot = radioSpot;
            _radioGO.transform.parent = _radioSpot.gameObject.transform;
            _radioGO.transform.position = _radioSpot.gameObject.transform.position;
            _radioGO.transform.rotation = _radioSpot.gameObject.transform.rotation;
            
            Collider collider = _radioGO.GetComponent<Collider>();
            collider.enabled = true;
            
            _hasRadio = false;
            _hasSomethingInHand = false;
            _canInteract = false;
            StartCoroutine(InteractionTimer());
        }

        if (_isLookingAtRadio)
        {
            
        }

        if (_canMove)
        {
            Move();
        }
        
        Look();
    }

    private void Move()
    {
        _dir = _moveInput.ReadValue<Vector2>();

        Vector3 movementDir = _camera.transform.forward * _dir.y + _camera.transform.right * _dir.x;

        movementDir.y = 0;

        _cc.Move(movementDir * _speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, _camera.transform.eulerAngles.y, 0f);
    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _distance, _interactableLayer))
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
                    PickUpPlant(plant);
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
            } else if (hit.collider.tag == "Radio" && _canInteract)
            {
                //If RadioSpot is not null, we need to remove music
                if (_interactInput.triggered && !_hasSomethingInHand && _canInteract)
                {
                    if (_currentRadioSpot != null)
                    {
                        _currentRadioSpot.MusicPlaying(MusicType.None);
                    }

                    RadioDataController radioDataController = hit.collider.gameObject.GetComponent<RadioDataController>();
                    radioDataController.radioSpot = null;
                    PickUpRadio(hit.collider.gameObject);
                    _radioGO = hit.collider.gameObject;
                    _hasSomethingInHand = true;
                    _hasRadio = true;
                    _canInteract = false;
                    StartCoroutine(InteractionTimer());
                } else if (_useInput.triggered)
                {
                    GameObject radio = hit.collider.gameObject;

                    GameEvents.OnLookAtRadioEvent();

                    _canMove = false;
                    _isLookingAtRadio = true;
                    
                    _moveInput.Disable();
                    _useInput.Disable();
                    _interactInput.Disable();
                    SwitchToRadioUI();
                }
            }
        }
        else
        {
            if (_interactInput.triggered && _hasSomethingInHand  && _canInteract)
            {
                //Throw it
                ThrowObject();
                _hasRadio = false;
                _hasSomethingInHand = false;
                _plantsInHand.Clear();
                _canInteract = false;
                StartCoroutine(InteractionTimer());
            }
        }
    }

    private void SwitchToRadioUI()
    {
        Camera.main.enabled = false;
        _camera.enabled = false;
        _hud.SetActive(false);
    }

    private void ThrowObject()
    {
        _holdSocket.DetachChildren();
        if (_hasRadio)
        {
            Rigidbody _rb = _radioGO.GetComponent<Rigidbody>();
            _rb.isKinematic = false;
            _rb.AddForce(_camera.transform.forward*_throwForce, ForceMode.Impulse);

            Collider collider = _radioGO.GetComponent<Collider>();
            collider.enabled = true;
        }

        foreach (var plantController in _plantsInHand)
        {
            Rigidbody _rb = plantController.GetComponent<Rigidbody>();
            _rb.isKinematic = false;
            _rb.AddForce(_camera.transform.forward*_throwForce, ForceMode.Impulse);

            Collider collider = plantController.GetComponent<Collider>();
            collider.enabled = true;
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
            slot.plantController = ctrl;
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
    
    private void PickUpRadio(GameObject radio)
    {
        //Switch Rigidbody to kinematic
        Rigidbody _rb = radio.GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        
        // Deactivate Collider
        Collider collider = radio.GetComponent<Collider>();
        collider.enabled = false;
        
        //Parent to hand
        radio.transform.parent = _holdSocket;
        radio.transform.position = _holdSocket.position;
        radio.transform.localPosition = Vector3.zero;
        radio.transform.localRotation = _holdSocket.rotation;
    }

    private void PickUpPlant(GameObject plant)
    {
        PlantController plantController = plant.GetComponent<PlantController>();
        plantController.PickedUp();
        //Switch Rigidbody to kinematic
        Rigidbody _rb = plant.GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        
        //Parent to hand
        plant.transform.parent = _holdSocket;
        plant.transform.position = _holdSocket.position;
        plant.transform.localPosition = Vector3.zero;
        plant.transform.localRotation = _holdSocket.rotation;
        
        // Deactivate Collider
        Collider collider = plant.GetComponent<Collider>();
        collider.enabled = false;
        //Add to our hold plants
        _plantsInHand.Add(plantController);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RadioSpot")
        {
            _inGardenBoxZone = true;
            _radioSpot = other.GetComponentInChildren<RadioSpot>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RadioSpot")
        {
            _inGardenBoxZone = false;
            _radioSpot = null;
        }
    }
    
    private void OnStopLookAtRadioEvent(MusicType musicType)
    {
        _canMove = true;
        _isLookingAtRadio = false;

        _camera.enabled = true;
        _hud.SetActive(true);
                    
        _moveInput.Enable();
        _useInput.Enable();
        _interactInput.Enable();
    }

    private void OnDestroy()
    {
        GameEvents.OnOrderDoneEvent -= OnOrderDoneEvent;
    }
}
