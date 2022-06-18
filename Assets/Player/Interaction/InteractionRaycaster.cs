using UnityEngine;

public class InteractionRaycaster : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _interactRange;
    [SerializeField] private LayerMask _interactableMask;


    public Camera Camera => _camera;
    public Transform CurrentInteractionTarget { get; private set; }


    private void Update()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));        
        if(Physics.Raycast(ray, out RaycastHit hit, _interactRange, _interactableMask))
        {           
            if(CurrentInteractionTarget != hit.transform)
            { 
                CurrentInteractionTarget = hit.transform;
                HighLight();
            }
        }
        else
        {
            CurrentInteractionTarget = null;
        }
    }


    private void HighLight()
    {

    }
}
