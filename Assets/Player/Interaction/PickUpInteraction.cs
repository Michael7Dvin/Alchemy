using System;
using UnityEngine;

[RequireComponent(typeof(InteractionRaycaster))]
public class PickUpInteraction : MonoBehaviour
{
    private PlayerInput _playerInput;

    [SerializeField] private Transform _playerPickUpPoint;    
    [SerializeField] private PickUpable _currentPickUp;

    [SerializeField] private float _levitationSpeed;
    [SerializeField] private float _droppingVeclocityReduction;

    [SerializeField] private float _throwingPower;
    [SerializeField] private float _maxThrowingVectorMagnitude;

    [SerializeField] private float _maxRotationSpeed;

    private InteractionRaycaster _interactionRaycaster;    

    public event Action RotationStarted;
    public event Action RotationPerformed;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _interactionRaycaster = GetComponent<InteractionRaycaster>();

        _playerInput.Interaction.Interact.performed += context => OnInteractInput();
        _playerInput.Interaction.DropItem.performed += context => Drop();
        _playerInput.Interaction.ThrowItem.performed += context => Throw();

        _playerInput.Interaction.StrartItemRotation.started += context =>
        {
            if(_currentPickUp != null)
            {
                RotationStarted?.Invoke();
            }
        };
        _playerInput.Interaction.StrartItemRotation.canceled += context => RotationPerformed();
    }

    private void OnEnable() => _playerInput.Enable();
    private void OnDisable() => _playerInput.Disable();

    private void FixedUpdate()
    {
        if(_currentPickUp != null)
        {
            Levitate();
        }
        
        if(_currentPickUp != null && _playerInput.Interaction.StrartItemRotation.IsPressed())
        {
            Rotate();
        }
    }  

    private void OnInteractInput()
    {
        if(_interactionRaycaster.CurrentInteractionTarget != null)
        {
            if (_interactionRaycaster.CurrentInteractionTarget.TryGetComponent(out PickUpable pickUpable))
            {
                SwapItems(pickUpable);
            }
        }
    }

    private void PickUp(PickUpable pickUpable)
    {
        _currentPickUp = pickUpable;
        _currentPickUp.Rigidbody.useGravity = false;
    }

    private void Drop()
    { 
        if (_currentPickUp != null)
        {           
            _currentPickUp.Rigidbody.useGravity = true;

            Vector3 reducedVelocity = _currentPickUp.Rigidbody.velocity / _droppingVeclocityReduction;
            _currentPickUp.Rigidbody.velocity = reducedVelocity;

            _currentPickUp = null;
        }
    }   
    
    private void SwapItems(PickUpable newPickUpable)
    {
        if (_currentPickUp != null)
        {
            Drop();
        }

        PickUp(newPickUpable);
    }

    private void Levitate()
    {
        Vector3 directionToPoint = _playerPickUpPoint.position - _currentPickUp.transform.position;
        _currentPickUp.Rigidbody.velocity = _levitationSpeed * directionToPoint;
    }

    private void Throw()
    {
        if (_currentPickUp != null)
        {
            _currentPickUp.Rigidbody.useGravity = true;
            
            Vector3 directionToPoint = _currentPickUp.transform.position - _interactionRaycaster.Camera.transform.position;

            Vector3 acceleration = (_throwingPower * directionToPoint) / _currentPickUp.Rigidbody.mass;
            acceleration = Vector3.ClampMagnitude(acceleration, _maxThrowingVectorMagnitude);
            
            _currentPickUp.Rigidbody.velocity = acceleration;

            _currentPickUp = null;
        }
    }

    private void Rotate()
    {
        Vector2 _inputRotation = _playerInput.Interaction.ItemRotation.ReadValue<Vector2>();
        _inputRotation = Vector2.ClampMagnitude(_inputRotation, _maxRotationSpeed);

        _currentPickUp.transform.Rotate(0, -_inputRotation.x, 0, Space.World);      
        _currentPickUp.transform.RotateAround(_currentPickUp.transform.position, transform.right, _inputRotation.y);
    }
}
