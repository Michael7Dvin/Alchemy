using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Stove : MonoBehaviour, IHaveRealTimePhysicalProcesses
{
    // If more than 2 states, create State machine 
    private enum StoveState
    {
        Ignited = 0,
        Extinguished = 1
    }

    [SerializeField] private float _heatLossFactor;
    [SerializeField] private List<Fuel> _fuel = new List<Fuel>();
    [SerializeField, Tooltip("Kilograms")] private float _fuelCapacity;
    
    [SerializeField] private StoveState _currentState = StoveState.Extinguished;
    
    private Heatable _cauldron;

    // Change when player controller be added
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


    [Inject]
    private void Construct(Cauldron cauldron)
    {
        _cauldron = cauldron;
    }

    private void Start()
    {
        StartCoroutine(ImplementRealTimePhysicalProcesses());

        //change when player controller be added
        if(_fuelToAdd != null)
            AddFuel(_fuelToAdd);

        Ignite();
    }
                  
    private void OnDisable()
    {
        foreach(Fuel fuel in _fuel)
        {
            fuel.BurnOut -= RemoveFuel;
        }
    }


    public IEnumerator ImplementRealTimePhysicalProcesses()
    {   
        if(_currentState == StoveState.Ignited)
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
            fuel.BurnOut += RemoveFuel;
            _fuel.Add(fuel);
        }
    }

    private void RemoveFuel(Fuel fuel)
    {
        fuel.BurnOut -= RemoveFuel;
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

        if(_cauldron != null)
        {
            _cauldron.HeatUp((amountOfHeat - (amountOfHeat * _heatLossFactor)) * PhysicalProcessesSimulation.SpeedCorrection);
        }
    }
}
