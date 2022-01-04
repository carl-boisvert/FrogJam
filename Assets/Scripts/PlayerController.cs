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
    
    private Transform _cameraTransform;
    private MovementControl _moveControl;
    private InputAction _moveInput;
    private InputAction _jumpInput;

    private Vector2 _dir;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        _moveControl = new MovementControl();
        
        _moveInput = _moveControl.Player.Move;
        _moveInput.Enable();

        _jumpInput = _moveControl.Player.Jump;
        _jumpInput.Enable();
        
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
        
        Debug.Log(movementDir);

        _cc.Move(movementDir * _speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f);
    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, _distance, _interactableLayer))
        {
            Debug.Log("Hit");
        }
    }
}
