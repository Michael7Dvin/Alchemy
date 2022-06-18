using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    private enum MovementStates
    {
        Walking = 0,
        Running = 1,
        Crouching = 2
    }
    
    private Vector3 _localMoveDirection;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _crouchSpeed;

    private float _standingVerticalScale;
    [SerializeField] private float _crouchingVerticalScale;
    
    private MovementStates _currentState = MovementStates.Walking;

    private PlayerInput _playerInput;
    private CharacterController _characterController;


    private void Awake()
    {
        _standingVerticalScale = transform.localScale.y;
        
        _characterController = GetComponent<CharacterController>();
        _playerInput = new PlayerInput();

        _playerInput.Movement.HorizontalMovement.started += OnHorizontalMovementInput;
        _playerInput.Movement.HorizontalMovement.performed += OnHorizontalMovementInput;
        _playerInput.Movement.HorizontalMovement.canceled += OnHorizontalMovementInput;
        
        _playerInput.Movement.Run.started += context => StartRunning();        
        _playerInput.Movement.Run.canceled += context => StopRunning();

        _playerInput.Movement.Crouch.started += context => StartCrouching();
        _playerInput.Movement.Crouch.canceled += context => StopCrouching();
    }

    private void OnEnable() => _playerInput.Enable();
    private void OnDisable() => _playerInput.Disable();
                
    private void Update()
    {
        Vector3 gravityDirection = new Vector3(0f, Physics.gravity.y, 0f);
        _characterController.Move(gravityDirection * Time.deltaTime);

        if (_currentState == MovementStates.Walking)
        {
            Move(_walkSpeed);
        }
        else if (_currentState == MovementStates.Running)
        {
            Move(_runSpeed);
        }
        else if (_currentState == MovementStates.Crouching)
        {
            Move(_crouchSpeed);
        }
    }


    private void OnHorizontalMovementInput(InputAction.CallbackContext context)
    {
        Vector2 inputMoveDirection = context.ReadValue<Vector2>();       
        _localMoveDirection = new Vector3(inputMoveDirection.x, 0, inputMoveDirection.y);
    }   

    private void Move(float moveSpeed)
    {
        Vector3 worldSpaceMoveDirection = transform.TransformDirection(_localMoveDirection);
        _characterController.Move(moveSpeed * Time.deltaTime * worldSpaceMoveDirection);
    }

    private void StartCrouching()
    {
        _currentState = MovementStates.Crouching;
        transform.localScale = new Vector3(transform.localScale.x, _crouchingVerticalScale, transform.localScale.z);
    }
    private void StartRunning()
    {
        if(_currentState == MovementStates.Crouching)
        {
            StopCrouching();
        }
        _currentState = MovementStates.Running;
    }

    private void StopRunning()
    {
        if(_currentState == MovementStates.Running)
        {
            _currentState = MovementStates.Walking;
        }
    }

    private void StopCrouching()
    {
        if(_currentState == MovementStates.Crouching)
        {
            _currentState = MovementStates.Walking;
        }

        transform.localScale = new Vector3(transform.localScale.x, _standingVerticalScale, transform.localScale.z);
    }

}