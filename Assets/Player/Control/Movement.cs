using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    private PlayerInput _playerInput;

    [SerializeField] private float _moveSpeed;
    private Vector3 _localMoveDirection;

    private CharacterController _characterController;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = new PlayerInput();

        _playerInput.Movement.HorizontalMovement.started += OnHorizontalMovementInput;
        _playerInput.Movement.HorizontalMovement.performed += OnHorizontalMovementInput;
        _playerInput.Movement.HorizontalMovement.canceled += OnHorizontalMovementInput;        
    }

    private void OnEnable() => _playerInput.Enable();
    private void OnDisable() => _playerInput.Disable();
                
    private void Update()
    {
        Vector3 worldSpaceMoveDirection = transform.TransformDirection(_localMoveDirection);
        _characterController.Move(worldSpaceMoveDirection * _moveSpeed * Time.deltaTime);
    }

    private void OnHorizontalMovementInput(InputAction.CallbackContext context)
    {
        Vector2 inputMoveDirection = context.ReadValue<Vector2>();
        _localMoveDirection = new Vector3(inputMoveDirection.x, 0, inputMoveDirection.y);        
    }   
}