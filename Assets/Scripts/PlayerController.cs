using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float _speed;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Interaction")] 
    [SerializeField]
    private GameObject _hud;

    [SerializeField] private float _distance;
    [SerializeField] private float _throwForce;
    [SerializeField] private Transform _holdSocket;
    [SerializeField] private GameObject _radioHologram;

    [Header("Link")] 
    [SerializeField] private CharacterController _cc;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private GameManager _gameManager;

    [Header("Sound")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _dipPaint;
    [SerializeField] private AudioClip _grabSound;
    [SerializeField] private AudioClip _refillWaterSound;
    [SerializeField] private AudioClip _openBookSound;
    [SerializeField] private List<AudioClip> _throwSounds;

    [Header("Plant")] 
    [SerializeField] private GameObject _plantPrefab;
    [SerializeField] private GameObject _seedPrefab;

    [Header("Camera")] 
    [SerializeField] private Camera _camera;
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private CinemachineVirtualCamera _jumbotronCamera;

    [Header("State variable")] [SerializeField]
    private bool _hasSomethingInHand;

    [SerializeField] private PlantData _currentSeed;
    [SerializeField] private GameObject _currentSeedGo;
    [SerializeField] private List<PlantController> _plantsInHand = new List<PlantController>();
    [SerializeField] private bool _hasWaterSpray;
    [SerializeField] private GameObject _waterSprayGo;
    [SerializeField] private bool _hasFrog;
    [SerializeField] private GameObject _frogGo;
    [SerializeField] private bool _hasRadio;
    [SerializeField] private GameObject _radioGO;
    [SerializeField] private List<GameObject> _radioGOHolos;
    [SerializeField] private bool _inGardenBoxZone;
    [SerializeField] private List<RadioSpot> _radioSpots;
    [SerializeField] private RadioSpot _currentRadioSpot;
    [SerializeField] private bool _canMove = true;
    [SerializeField] private bool _inputDisabled = false;
    [SerializeField] private bool _isLookingAtRadio = false;
    [SerializeField] private GameObject _tooltipGo;
    [SerializeField] private TooltipData _tooltipData;


    private TooltipController _tooltipController;
    private MovementControl _moveControl;
    private LookControl _lookControl;
    private InputAction _moveInput;
    private InputAction _interactInput;
    private InputAction _useInput;
    private InputAction _throwInput;
    private InputAction _dropInput;
    private InputAction _mainMenuInput;

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

        _throwInput = _lookControl.Mouse.Throw;
        _throwInput.Enable();

        _dropInput = _moveControl.Player.Drop;
        _dropInput.Enable();

        _mainMenuInput = _lookControl.Mouse.MainMenu;
        _mainMenuInput.Enable();

        _tooltipController = _tooltipGo.GetComponent<TooltipController>();

        GameEvents.OnOrderDoneEvent += OnOrderDoneEvent;
        GameEvents.OnStopLookAtRadioEvent += OnStopLookAtRadioEvent;
        GameEvents.OnStopLookAtPlantopediaEvent += OnStopLookAtPlantopediaEvent;
        GameEvents.OnGameEndEvent += OnGameEndEvent;
        GameEvents.OnGameContinueEvent += OnGameContinueEvent;
    }

    private void OnGameContinueEvent()
    {
        SwitchToPlayerCamera();
        Cursor.lockState = CursorLockMode.Locked;
        _hud.SetActive(true);
    }

    private void OnGameEndEvent()
    {
        SwitchToJumbotronCamera();
        Cursor.lockState = CursorLockMode.Confined;
        _hud.SetActive(false);
    }

    private void OnStopLookAtPlantopediaEvent()
    {
        EnableInputs();
    }

    private void OnOrderDoneEvent(Order order)
    {
        _plantsInHand.Clear();
        _hasSomethingInHand = false;
    }

    private void Update()
    {
        if (_mainMenuInput.enabled && _mainMenuInput.triggered)
        {
            _playerCamera.Priority = 5;
            _mainMenuInput.Disable();
            GameEvents.OnGameStartEvent();
        }

        _holdSocket.forward = -_camera.transform.forward;

        if (_canMove)
        {
            Move();
            Look();
        }
    }

    private void Move()
    {
        _dir = _moveInput.ReadValue<Vector2>();

        Vector3 movementDir = _camera.transform.forward * _dir.y + _camera.transform.right * _dir.x;

        movementDir.y = -9.61f;

        // Lock Y position after move
        _cc.Move(movementDir * _speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, _camera.transform.eulerAngles.y, 0f);
    }

    private void Look()
    {
        RaycastHit hit;

        _tooltipController.hideToolTips();
        _tooltipGo.SetActive(false);

        if (!_inputDisabled)
        {
            if (_interactInput.triggered && _hasWaterSpray)
            {
                WaterSpray waterSpray = _waterSprayGo.GetComponent<WaterSpray>();
                waterSpray.Spray();
            }

            if (_dropInput.triggered && _hasSomethingInHand)
            {
                //Drop object
                if (_hasFrog)
                {
                    DropObject(_frogGo, false);
                    _frogGo = null;
                }
                else if (_hasRadio)
                {
                    DropObject(_radioGO, false);
                    _radioGO = null;
                }
                else if (_hasWaterSpray)
                {
                    DropObject(_waterSprayGo, false);
                    _waterSprayGo = null;
                }
                else if (_plantsInHand.Count > 0)
                {
                    foreach (var plantController in _plantsInHand)
                    {
                        DropObject(plantController.gameObject, false);
                    }
                    _plantsInHand.Clear();
                }


                _hasSomethingInHand = false;
                _hasFrog = false;
                _hasWaterSpray = false;
                _hasRadio = false;
            }

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _distance,
                _interactableLayer))
            {
                //Pick Up Interaction
                if (hit.collider.tag == "GardenSlot")
                {
                    if (_currentSeed != null)
                    {
                        ShowTooltips("GardenSlot");
                    }

                    if (_interactInput.triggered && _currentSeed != null)
                    {
                        GardenSlot gs = hit.collider.gameObject.GetComponent<GardenSlot>();
                        Plant(gs);
                        _hasSomethingInHand = false;
                        Destroy(_currentSeedGo);
                    }
                }
                else if (hit.collider.tag == "SeedBag")
                {
                    ShowTooltips("SeedBag");
                    if (_interactInput.triggered)
                    {
                        _audioSource.PlayOneShot(_grabSound);
                        SeedBag bag = hit.collider.gameObject.GetComponent<SeedBag>();
                        _currentSeed = bag.plantData;
                        _hasSomethingInHand = true;
                        _currentSeedGo = Instantiate(_seedPrefab);
                        AttacheToHand(_currentSeedGo);
                    }
                }
                else if (hit.collider.tag == "Plant")
                {
                    ShowTooltips("Plant");
                    if (_interactInput.triggered && !_hasSomethingInHand)
                    {
                        _audioSource.PlayOneShot(_grabSound);
                        GameObject plant = hit.collider.gameObject;
                        PickUpPlant(plant);
                        _hasSomethingInHand = true;
                    }
                }
                else if (hit.collider.tag == "Kiosque")
                {
                    if (_plantsInHand.Count > 0)
                    {
                        ShowTooltips("Kiosque");
                        if (_interactInput.triggered)
                        {
                            
                                SellPlant();
                            
                        }
                    }
                }
                else if (hit.collider.tag == "Radio")
                {
                    ShowTooltips("Radio");
                    if (_interactInput.triggered && !_hasSomethingInHand)
                    {
                        foreach (var radioSpot in _radioSpots)
                        {
                            _radioGOHolos.Add(Instantiate(_radioHologram, radioSpot.transform));
                        }
                        
                        if (_currentRadioSpot != null)
                        {
                            _currentRadioSpot.MusicPlaying(MusicType.None);
                            _currentRadioSpot.GetComponentInParent<Collider>().enabled = true;
                            _currentRadioSpot = null;
                        }

                        _audioSource.PlayOneShot(_grabSound);
                        RadioDataController radioDataController =
                            hit.collider.gameObject.GetComponent<RadioDataController>();
                        radioDataController.radioSpot = null;
                        PickUpRadio(hit.collider.gameObject);
                        _radioGO = hit.collider.gameObject;
                        _hasSomethingInHand = true;
                        _hasRadio = true;
                    }
                    else if (_useInput.triggered)
                    {
                        GameObject radio = hit.collider.gameObject;

                        GameEvents.OnLookAtRadioEvent();

                        _isLookingAtRadio = true;

                        DisableInputs();
                        SwitchToRadioUI();
                    }
                }
                else if (hit.collider.tag == "Frog")
                {
                    ShowTooltips("Frog");
                    if (_interactInput.triggered && !_hasSomethingInHand)
                    {
                        _audioSource.PlayOneShot(_grabSound);
                        PickUpFrog(hit.collider.gameObject);
                        _frogGo = hit.collider.gameObject;
                        _hasFrog = true;
                        _hasSomethingInHand = true;
                    }
                }
                else if (hit.collider.tag == "Bin")
                {
                    if (_plantsInHand.Count > 0)
                    {
                        ShowTooltips("Bin");
                    }

                    if (_interactInput.triggered)
                    {
                        if (_plantsInHand.Count > 0)
                        {
                            foreach (var plantController in _plantsInHand)
                            {
                                Destroy(plantController.gameObject);
                            }

                            _plantsInHand.Clear();
                            _hasSomethingInHand = false;
                        }
                    }
                }
                else if (hit.collider.tag == "WaterSpray")
                {
                    ShowTooltips("WaterSpray");
                    if (_interactInput.triggered && !_hasSomethingInHand)
                    {
                        _audioSource.PlayOneShot(_grabSound);
                        GameObject waterSpray = hit.collider.gameObject;
                        _waterSprayGo = waterSpray;
                        PickUpWaterSpray(waterSpray);
                        _hasWaterSpray = true;
                        _hasSomethingInHand = true;
                        //_canInteract = false;
                        //StartCoroutine(InteractionTimer());
                    }
                }
                else if (hit.collider.tag == "Sink")
                {
                    if (_hasWaterSpray)
                    {
                        ShowTooltips("Sink");
                    }

                    if (_interactInput.triggered && _hasWaterSpray)
                    {
                        _audioSource.PlayOneShot(_refillWaterSound);
                        WaterSpray waterSpray = _waterSprayGo.GetComponent<WaterSpray>();
                        waterSpray.Refill();
                    }
                }
                else if (hit.collider.tag == "Paint")
                {
                    if (_plantsInHand.Count > 0)
                    {
                        ShowTooltips("Paint");
                        if (_interactInput.triggered)
                        {
                            _audioSource.PlayOneShot(_dipPaint);
                            Paint paint = hit.collider.gameObject.GetComponent<Paint>();
                            _plantsInHand[0].Paint(paint.color);
                        }
                    }
                }
                else if (hit.collider.tag == "Book")
                {
                    ShowTooltips("Book");
                    if (_useInput.triggered)
                    {
                        _audioSource.PlayOneShot(_openBookSound);
                        DisableInputs();
                        PlantopediaController plantopedia =
                            hit.collider.gameObject.GetComponent<PlantopediaController>();
                        plantopedia.ShowPlantopedia();
                    }
                }
                else if (hit.collider.tag == "RadioSpot")
                {
                    if (_hasRadio)
                    {
                        ShowTooltips("RadioSpot");
                        if (_interactInput.triggered)
                        {
                            foreach (var holo in _radioGOHolos)
                            {
                                Destroy(holo);
                            }
                            
                            //Placer la radio
                            RadioSpot radioSpot = hit.collider.gameObject.GetComponentInChildren<RadioSpot>();
                            RadioDataController radioDataController = _radioGO.GetComponent<RadioDataController>();
                            radioSpot.MusicPlaying(radioDataController._musicPlaying);
                            _currentRadioSpot = radioSpot;
                            _currentRadioSpot.GetComponentInParent<Collider>().enabled = false;
                            radioDataController.radioSpot = radioSpot;
                            _radioGO.transform.parent = radioSpot.gameObject.transform;
                            _radioGO.transform.position = radioSpot.gameObject.transform.position;
                            _radioGO.transform.rotation = radioSpot.gameObject.transform.rotation;
            
                            Collider collider = _radioGO.GetComponent<Collider>();
                            collider.enabled = true;
            
                            _hasRadio = false;
                            _hasSomethingInHand = false;
                            _radioGOHolos.Clear();
                        }
                    }
                }
            }
            else
            {
                //Other Interaction
                if (_throwInput.triggered && _hasSomethingInHand)
                {
                    //Throw it
                    int roll = Random.Range(0, _throwSounds.Count);
                    _audioSource.PlayOneShot(_throwSounds[roll]);
                    ThrowObject();
                    _hasRadio = false;
                    _hasFrog = false;
                    _hasSomethingInHand = false;
                    _hasWaterSpray = false;
                    _plantsInHand.Clear();
                }
            }
        }
    }

    private void SwitchToJumbotronCamera()
    {
        DisableInputs();

        _playerCamera.Priority = 0;
        _jumbotronCamera.Priority = 5;
    }
    
    private void SwitchToPlayerCamera()
    {
        EnableInputs();

        _playerCamera.Priority = 5;
        _jumbotronCamera.Priority = 0;
    }

    private void DisableInputs()
    {
        _canMove = false;
        _inputDisabled = true;
        _moveInput.Disable();
        _interactInput.Disable();
        _useInput.Disable();
        _throwInput.Disable();
        _dropInput.Disable();
    }

    private void EnableInputs()
    {
        _canMove = true;
        _inputDisabled = false;
        _moveInput.Enable();
        _interactInput.Enable();
        _useInput.Enable();
        _throwInput.Enable();
        _dropInput.Enable();
    }

    private void ShowTooltips(string key)
    {
        List<TooltipInfo> datas = _tooltipData.tooltips.FindAll(tooltipInfo => tooltipInfo.tag == key);
        if (datas.Count > 0)
        {
            _tooltipController.SetInfoTooltip1(datas[0]);
        }

        if (datas.Count > 1)
        {
            _tooltipController.SetInfoTooltip2(datas[1]);
        }

        _tooltipGo.SetActive(true);
    }

    private void PickUpWaterSpray(GameObject waterSpray)
    {
        //Switch Rigidbody to kinematic
        Rigidbody _rb = waterSpray.GetComponent<Rigidbody>();
        _rb.isKinematic = true;

        //Parent to hand
        AttacheToHand(waterSpray, false);

        // Deactivate Collider
        Collider collider = waterSpray.GetComponent<Collider>();
        collider.enabled = false;
    }

    private void SwitchToRadioUI()
    {
        Camera.main.enabled = false;
        _camera.enabled = false;
        _hud.SetActive(false);
    }

    private void DropObject(GameObject go, bool throwObject = true)
    {
        Rigidbody _rb = go.GetComponent<Rigidbody>();
        _rb.isKinematic = false;

        Collider collider = go.GetComponent<Collider>();
        collider.enabled = true;

        if (throwObject)
        {
            _rb.AddForce(_camera.transform.forward * _throwForce, ForceMode.Impulse);
        }
        else
        {
            go.transform.parent = null;
            go.transform.position = new Vector3(go.transform.position.x, 0, go.transform.position.z);
        }
    }

    private void ThrowObject()
    {
        _holdSocket.DetachChildren();
        if (_hasRadio)
        {
            DropObject(_radioGO);
            _radioGO = null;
        }

        if (_hasFrog)
        {
            Rigidbody _rb = _frogGo.GetComponent<Rigidbody>();
            _rb.isKinematic = false;
            _rb.AddForce(_camera.transform.forward * _throwForce, ForceMode.Impulse);

            Collider collider = _frogGo.GetComponent<SphereCollider>();
            collider.enabled = true;

            collider = _frogGo.GetComponent<BoxCollider>();
            collider.enabled = true;
            _frogGo = null;
        }

        if (_hasWaterSpray)
        {
            DropObject(_waterSprayGo);
            _waterSprayGo = null;
        }

        foreach (var plantController in _plantsInHand)
        {
            DropObject(plantController.gameObject);
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

    private void PickUpFrog(GameObject frog)
    {
        Rigidbody _rb = frog.GetComponent<Rigidbody>();
        _rb.isKinematic = true;

        Collider collider = frog.GetComponent<BoxCollider>();
        collider.enabled = false;

        FrogController frogController = frog.GetComponent<FrogController>();
        frogController.PickedUp();

        //Parent to hand
        AttacheToHand(frog);
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
        AttacheToHand(radio);
    }

    private void PickUpPlant(GameObject plant)
    {
        PlantController plantController = plant.GetComponent<PlantController>();
        plantController.PickedUp();
        //Switch Rigidbody to kinematic
        Rigidbody _rb = plant.GetComponent<Rigidbody>();
        _rb.isKinematic = true;

        //Parent to hand
        AttacheToHand(plant, false);

        // Deactivate Collider
        Collider collider = plant.GetComponent<Collider>();
        collider.enabled = false;
        //Add to our hold plants
        _plantsInHand.Add(plantController);
    }


    private void OnStopLookAtRadioEvent(MusicType musicType)
    {
        _canMove = true;
        _isLookingAtRadio = false;

        _camera.enabled = true;
        _hud.SetActive(true);

        EnableInputs();
    }

    private void AttacheToHand(GameObject go, bool facingPlayer = true)
    {
        //Parent to hand
        go.transform.parent = _holdSocket;
        go.transform.position = _holdSocket.position;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        if (!facingPlayer)
        {
            go.transform.rotation *= Quaternion.Euler(0, 180f, 0);
        }
    }

    private void OnDestroy()
    {
        GameEvents.OnOrderDoneEvent -= OnOrderDoneEvent;
    }
}