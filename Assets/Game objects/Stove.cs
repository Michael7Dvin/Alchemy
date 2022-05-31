using System.Collections.Generic;
using UnityEngine;

public abstract class Stove : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [SerializeField] private List<Fuel> _fuel = new();
    [SerializeField] private float _heatLossFactor;

    [Tooltip("Kilograms")]
    [SerializeField] private float _fuelCapacity;

    public string Name => _name;
    public string Description => _description;
    public List<Fuel> Fuel => _fuel;
    public float HeatLossFactor => _heatLossFactor;
    public float FuelCapacity => _fuelCapacity;

    private void AddFuel(Fuel fuel)
    {
        
    }

    private void Light()
    {

    }

    private void BurnFuel()
    {

    }

    private void Extinguish()
    {

    }

}
