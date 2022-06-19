using UnityEngine;

public class SwitchInteraction : BaseInteraction
{
    protected override void Awake()
    {
        base.Awake();

        _playerInput.Interaction.Interact.performed += context => OnInteractInput();
    }


    protected override void OnInteractInput()
    {
        if (_interactionRaycaster.CurrentInteractionTarget != null)
        {
            if (_interactionRaycaster.CurrentInteractionTarget.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }
}


