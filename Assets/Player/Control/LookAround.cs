using UnityEngine;
using UnityEngine.InputSystem;

public class LookAround : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Vector2 _inputLookRotation;

    [SerializeField] float _sensivityX;
    [SerializeField] float _sensivityY;

    private float _rotationX;
    private float _rotationY;

    [SerializeField] private Transform _camera;    

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.LookAround.Look.started += OnLookAroundInput;
        _playerInput.LookAround.Look.performed += OnLookAroundInput;
        _playerInput.LookAround.Look.canceled += OnLookAroundInput;
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

    private void OnLookAroundInput(InputAction.CallbackContext context)
    {
        _inputLookRotation = context.ReadValue<Vector2>() * _sensivityY;
    }

    private void Rotate()
    {      
        _rotationX -= _inputLookRotation.y;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

        _rotationY = _inputLookRotation.x;

        _camera.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * _rotationY);
    }
}
