using UnityEngine;
using UnityEngine.InputSystem;

public class LookAround : MonoBehaviour
{
    [SerializeField] float _sensivityX;
    [SerializeField] float _sensivityY;

    private float _rotationX;
    private float _rotationY;
    private Vector2 _inputLookRotation;

    private PlayerInput _playerInput;
    [SerializeField] private Transform _playerBody;


    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.LookAround.Look.started += OnLookAroundinput;
        _playerInput.LookAround.Look.performed += OnLookAroundinput;
        _playerInput.LookAround.Look.canceled += OnLookAroundinput;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() => _playerInput.Enable();
    private void OnDisable() => _playerInput.Disable();

    private void Update()
    {
        Rotate();
    }

    private void OnLookAroundinput(InputAction.CallbackContext context)
    {
        _inputLookRotation = context.ReadValue<Vector2>() * _sensivityY;
    }

    private void Rotate()
    {      
        _rotationX -= _inputLookRotation.y;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

        _rotationY = _inputLookRotation.x;

        transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        _playerBody.Rotate(Vector3.up * _rotationY);
    }
}
