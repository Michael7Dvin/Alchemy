using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PickUpInteraction))]
public class LookAround : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Vector2 _inputLookRotation;
    private bool _isLocked;

    [SerializeField] float _sensivityX;
    [SerializeField] float _sensivityY;

    private float _rotationX;
    private float _rotationY;

    [SerializeField] private Transform _camera;    
    private PickUpInteraction _pickUpInteraction;
    

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _pickUpInteraction = GetComponent<PickUpInteraction>();
        _pickUpInteraction.RotationStarted += () => _isLocked = true;
        _pickUpInteraction.RotationPerformed += () => _isLocked = false;

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
        if(_isLocked == false)
        {
            Rotate();
        }
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
