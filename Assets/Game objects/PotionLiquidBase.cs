using UnityEngine;

public abstract class PotionLiquidBase : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [Tooltip("Millilitres")]
    [SerializeField] private float _volume;

    [Tooltip("Megajoule / Kilograms * Kelvin")]
    [SerializeField] private float _specificHeat;

    [SerializeField] private float _temperature;
    [SerializeField] private float _boilingTemperature;

    public string Name => _name;
    public string Description => _description;
    public float Volume => _volume;
    public float SpecificHeat => _specificHeat;
    public float Temperature => _temperature;
    public float BoilingTemperature => _boilingTemperature;

}
