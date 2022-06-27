using UnityEngine;

public class Faucet : MonoBehaviour, IInteractable
{
    private enum FaucetStates
    {
        TurnedOff = 0,
        TurnedOn = 1,
    }

    [Tooltip("Liters(Kilograms) per second")]
    [SerializeField] private float _consumption;

    private FaucetStates _currentState;

    [SerializeField] CauldronLiquidProcesses _cauldron;


    private void Update()
    {
        if (_currentState == FaucetStates.TurnedOn)
        {
            Drain();
        }
    }


    public void Interact()
    {
        HandleState();
    }

    private void HandleState()
    {
        if (_currentState == FaucetStates.TurnedOn)
        {
            _currentState = FaucetStates.TurnedOff;
        }
        else if (_currentState == FaucetStates.TurnedOff)
        {
            _currentState = FaucetStates.TurnedOn;
        }
    }

    private void Drain()
    {
        float drainingLiquidMass = _consumption * Time.deltaTime;

        _cauldron.DrainLiquid(drainingLiquidMass);
    }
}
