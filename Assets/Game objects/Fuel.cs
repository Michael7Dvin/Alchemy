using UnityEngine;

public abstract class Fuel : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [Tooltip("Kilograms")]
    [SerializeField] private float _mass;

    [Tooltip("Megajoule / Kilograms")]
    [SerializeField] private float _heatingValue;

    [Tooltip("Burning time of 1kg of fuel (in sec.)")]
    [SerializeField] private float _heatingSpeed;


    public string Name => _name;
    public string Description => _description;
    public float Mass => _mass;
    public float HeatingValue => _heatingValue;
    public float HeatingSpeed => _heatingSpeed;

}
