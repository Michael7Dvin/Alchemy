
public class OneActionInteraction : BaseInteraction
{
    protected override void Awake()
    {
        base.Awake();
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


