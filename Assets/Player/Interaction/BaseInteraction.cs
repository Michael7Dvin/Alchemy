using UnityEngine;

[RequireComponent(typeof(InteractionRaycaster))]
public abstract class BaseInteraction : MonoBehaviour
{
    protected PlayerInput _playerInput;
    protected InteractionRaycaster _interactionRaycaster;


    protected virtual void Awake()
    {
        _playerInput = new PlayerInput();
        _interactionRaycaster = GetComponent<InteractionRaycaster>();
    }

    protected virtual void OnEnable() => _playerInput.Enable();
    protected virtual void OnDisable() => _playerInput.Disable();


    protected abstract void OnInteractInput();       
}
