using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If more than 2 states, create State machine 
public enum StoveState
{
    Ignited = 0,
    Extinguished = 1
}

public class Stove : MonoBehaviour, IHaveRealTimePhysicalProcesses
{
    [SerializeField] private StoveState _currentState = StoveState.Extinguished;

    //TEMPORARY, interfaces not serializable
    [SerializeField] private Cauldron _cauldron;
    private IHeatable _heatable;

    [SerializeField] private float _heatLossFactor;

    [SerializeField] private List<Fuel> _fuel = new();
    [SerializeField, Tooltip("Kilograms")] private float _fuelCapacity;

    //temp
    [SerializeField] private Fuel _fuelToAdd;

    private float GetAllFuelMass
    {
        get
        {
            float fuelMass = 0;
            foreach(Fuel fuel in _fuel)
            {
                fuelMass += fuel.Mass;
            }
            return fuelMass;
        }
    }
    private void Start()
    {
        StartCoroutine(ImplementRealTimePhysicalProcesses());

        //temp 
        if(_fuelToAdd != null)
            AddFuel(_fuelToAdd);

        Ignite();
        _heatable = _cauldron;
    }
                  
    private void OnDisable()
    {
        foreach(Fuel fuel in _fuel)
        {
            fuel.BurnOut -= RemoveFuelFromStove;
        }
    }

    public IEnumerator ImplementRealTimePhysicalProcesses()
    {     
        BurnFuel();
        yield return new WaitForSeconds(PhysicalProcessesSimulation.SpeedCorrection);
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }

    private void Ignite()
    {
        if(_currentState == StoveState.Extinguished & _fuel.Count > 0)
        {
            _currentState = StoveState.Ignited;
        }
    }
    private void Extinguish()
    {
        if(_currentState == StoveState.Ignited)
        {
            _currentState = StoveState.Extinguished;
        }
    }
    private void AddFuel(Fuel fuel)
    {
        if(GetAllFuelMass + fuel.Mass < _fuelCapacity)
        {
            fuel.BurnOut += RemoveFuelFromStove;
            _fuel.Add(fuel);
        }
    }
    private void RemoveFuelFromStove(Fuel fuel)
    {
        fuel.BurnOut -= RemoveFuelFromStove;
        _fuel.Remove(fuel);
    }
    private void BurnFuel()
    {
        List<Fuel> _fuelCopy = new(_fuel);

        if (_fuelCopy.Count == 0)
        {
            Extinguish();
            return;
        }

        float amountOfHeat = 0f;

        foreach (Fuel fuel in _fuelCopy)
        {
            amountOfHeat += fuel.Burn();
        }

        _heatable?.HeatUp((amountOfHeat - (amountOfHeat * _heatLossFactor)) * PhysicalProcessesSimulation.SpeedCorrection);
    }

}
