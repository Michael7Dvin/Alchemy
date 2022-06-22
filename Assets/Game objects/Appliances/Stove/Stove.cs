using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour, IInteractable
{
    private enum StoveStates
    {
        Extinguished = 0,
        Ignited = 1
    }

    [SerializeField] private float _heatLossFactor;    
    [SerializeField] private StoveStates _currentState = StoveStates.Extinguished;
    
    [SerializeField] private HeatingChamber _heatingChamber;
    [SerializeField] private Cauldron _cauldron;

        
    private void Update()
    {
        if (_currentState == StoveStates.Ignited)
        {
            BurnFuel();
        }
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
            _cauldron.HeatUp(amountOfHeat - (amountOfHeat * _heatLossFactor));
        }
    }
}
