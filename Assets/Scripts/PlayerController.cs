using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _fallSpeed;
    [Header("Interaction")]
    [SerializeField] private float _distance;
    [Header("Link")]
    [SerializeField] private CharacterController _cc;

    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private GameObject _plantPrefab;

    [SerializeField]
    private PlantData _currentSeed;
    
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
                if (_interactInput.triggered)
                {
                    GardenSlot gs = hit.collider.gameObject.GetComponent<GardenSlot>();
                    Plant(gs);
                }
            } else if (hit.collider.tag == "SeedBag")
            {
                if (_interactInput.triggered)
                {
                    SeedBag bag = hit.collider.gameObject.GetComponent<SeedBag>();
                    _currentSeed = bag.plantData;
                }
            }
        }
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
}
