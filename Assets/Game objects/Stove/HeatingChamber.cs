using System.Collections.Generic;
using UnityEngine;

public class HeatingChamber : MonoBehaviour
{
    [Tooltip("Kilograms")]
    [SerializeField] private float _fuelCapacity;
    [SerializeField] private List<Fuel> _fuel = new List<Fuel>();


    public List<Fuel> Fuel => _fuel;
    private float GetAllFuelMass
    {
        get
        {
            float fuelMass = 0;
            foreach (Fuel fuel in _fuel)
            {
                fuelMass += fuel.Mass;
            }
            return fuelMass;
        }
    }


    private void OnDisable()
    {
        foreach (Fuel fuel in _fuel)
        {
            fuel.BurnOut -= RemoveFuel;
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.TryGetComponent(out Fuel fuel))
        {
            AddFuel(fuel);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Fuel fuel))
        {
            RemoveFuel(fuel);
        }
    }


    private void AddFuel(Fuel fuel)
    {
        if (GetAllFuelMass + fuel.Mass < _fuelCapacity)
        {
            fuel.BurnOut += RemoveFuel;
            _fuel.Add(fuel);
        }
    }

    private void RemoveFuel(Fuel fuel)
    {
        fuel.BurnOut -= RemoveFuel;
        _fuel.Remove(fuel);
    }
}
