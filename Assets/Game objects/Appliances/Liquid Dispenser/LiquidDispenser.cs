using UnityEngine;

public class LiquidDispenser : MonoBehaviour, IInteractable
{
    private enum DispenserStates
    {
        TurnedOff = 0,
        TurnedOn = 1,
    }

    [Tooltip("Liters(Kilograms)")]
    [SerializeField] private float _dispenserVolume;

    [Tooltip("Liters(Kilograms) per second")]
    [SerializeField] private float _consumption;

    private float _fillingLiquidTemperature = 25f;
    private DispenserStates _currentState;

    [SerializeField] LiquidType _liquidType;
    [SerializeField] CauldronLiquidProcesses _cauldron;


    private void Update()
    {
        if (_currentState == DispenserStates.TurnedOn)
        {
            Fill();
        }
    }


    public void Interact()
    {
        HandleState();
    }

    private void HandleState()
    {
        if (_dispenserVolume == 0 || _currentState == DispenserStates.TurnedOn)
        {
            _currentState = DispenserStates.TurnedOff;
        }
        else if (_currentState == DispenserStates.TurnedOff)
        {
            _currentState = DispenserStates.TurnedOn;
        }
    }

    private void Fill()
    {
        float fillingLiquidMass = _consumption * Time.deltaTime;

        if(fillingLiquidMass > _dispenserVolume)
        {
            fillingLiquidMass -= _dispenserVolume; 
        }

        _cauldron.FillLiquid(fillingLiquidMass, _fillingLiquidTemperature);
    } 
}
