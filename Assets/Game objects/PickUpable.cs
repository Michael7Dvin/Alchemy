using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickUpable : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        transform.gameObject.layer = 7;

        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.angularDrag = 5f;
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }   
}
