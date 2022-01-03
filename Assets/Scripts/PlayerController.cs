using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _jumpMaxHeight;
    [Header("Link")]
    [SerializeField] private CharacterController _cc;
    
    private Transform _cameraTransform;
    private MovementControl _moveControl;
    private InputAction _moveInput;
    private InputAction _jumpInput;

    private Vector2 _dir;
    private bool _isJumping;
    private float _startHeight;
    
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
        _dir = _moveInput.ReadValue<Vector2>() * _speed * Time.deltaTime;
        
        Vector3 movementDir = _cameraTransform.forward * _dir.y + _cameraTransform.right * _dir.x;

        movementDir.y = 0;
        if (!_cc.isGrounded && !_isJumping)
        {
            movementDir.y -= _fallSpeed * Time.deltaTime;
        }
        else
        {
            if (_jumpInput.inProgress && !_isJumping)
            {
                movementDir.y += _speed * Time.deltaTime;
                _isJumping = true;
            }

            if (_isJumping && transform.position.y - _startHeight < _jumpMaxHeight)
            {
                movementDir.y += _speed * Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }

        _cc.Move(movementDir);

        if (_dir != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(_dir.x, _dir.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        }
    }
}
