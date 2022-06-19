using System;
using UnityEngine;

public class PickUpInteraction : BaseInteraction
{
    [SerializeField] private Transform _playerPickUpPoint;    
    [SerializeField] private PickUpable _currentPickUp;

    [SerializeField] private float _levitationSpeed;
    [SerializeField] private float _droppingVeclocityReduction;

    [SerializeField] private float _throwingPower;
    [SerializeField] private float _maxThrowingVectorMagnitude;

    private bool _isRotating;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxRotationSpeed;


    protected override void Awake()
    {
        base.Awake();

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
        _playerInput.Interaction.StrartItemRotation.canceled += context => RotationFinished();
    }

    private void FixedUpdate()
    {
        if(_currentPickUp != null)
        {
            Levitate();

            if(_isRotating == true)
            {
                Rotate();
            }
        }
    }

    private void Update()
    {
        HandleRotationInput();

        void HandleRotationInput()
        {
            if(_playerInput.Interaction.StrartItemRotation.IsPressed())
            {
                if(_isRotating == false)
                {
                    _isRotating = true;
                }
            }
            else if(_isRotating == true)
            {
                _isRotating = false;
            }
        }
    }


    public event Action RotationStarted;
    public event Action RotationFinished;


    protected override void OnInteractInput()
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
        Vector2 _inputRotation = _playerInput.Interaction.ItemRotation.ReadValue<Vector2>() * _rotationSpeed * Time.deltaTime;
        _inputRotation = Vector2.ClampMagnitude(_inputRotation, _maxRotationSpeed);

        Vector3 verticalInputRoatation = new Vector3(0, -_inputRotation.x, 0);
        _currentPickUp.Rigidbody.rotation = Quaternion.Euler(verticalInputRoatation) * _currentPickUp.Rigidbody.rotation; 

        _currentPickUp.Rigidbody.rotation = Quaternion.AngleAxis(_inputRotation.y, transform.right) * _currentPickUp.Rigidbody.rotation;
    }
}
