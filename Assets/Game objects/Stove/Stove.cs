using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Stove : MonoBehaviour, IHaveRealTimePhysicalProcesses, IInteractable
{
    private enum StoveStates
    {
        Ignited = 0,
        Extinguished = 1
    }

    [SerializeField] private float _heatLossFactor;    
    [SerializeField] private StoveStates _currentState = StoveStates.Extinguished;
    
    [SerializeField] private HeatingChamber _heatingChamber;
    private Heatable _cauldron;

    
    [Inject]
    private void Construct(Cauldron cauldron)
    {
        _cauldron = cauldron;
    }

    private void Start()
    {
        StartCoroutine(ImplementRealTimePhysicalProcesses());        
    }

    
    public IEnumerator ImplementRealTimePhysicalProcesses()
    {   
        if(_currentState == StoveStates.Ignited)
        {
            BurnFuel();
        }

        yield return new WaitForSeconds(PhysicalProcessesSimulation.SpeedCorrection);
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }

    public void Interact()
    {
        HandleState();
    }

    private void HandleState()
    {
        if (_heatingChamber.Fuel.Count == 0 || _currentState == StoveStates.Ignited)
        {
            Extinguish();
        }
        else if(_currentState == StoveStates.Extinguished)
        {
            Ignite();
        }
    }

    private void Ignite() => _currentState = StoveStates.Ignited;
    private void Extinguish() => _currentState = StoveStates.Extinguished;

    private void BurnFuel()
    {
        List<Fuel> _fuelCopy = new(_heatingChamber.Fuel);

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
