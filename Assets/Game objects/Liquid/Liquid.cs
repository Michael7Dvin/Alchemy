using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class Liquid : Heatable
{
    [SerializeReference] private LiquidType _liquidType;
    [SerializeField] private CauldronLiquidProcesses _cauldron;

    private BaseLiquidState _currentLiquidState;
    private List<BaseLiquidState> _liquidStates;

    public LiquidType LiquidType => _liquidType;
    public BaseLiquidState CurrentLiquidState => _currentLiquidState;

    protected void Start()
    {
        _liquidStates = new List<BaseLiquidState>()
        {
            new NoneLiquidState(this, _observableMass, _observableTemperature),
            new NormalLiquidState(this, _observableMass, _observableTemperature),
            new BoilingLiquidState(this, _observableMass, _observableTemperature)
        };
        SwitchLiquidState<NoneLiquidState>();
    }

    private void Update()
    {
        TransferHeatToAir();

        if (Temperature > _cauldron.Temperature)
        {
            TransferHeat(_cauldron);
        }
    }


    public void Fill(float fillingLiquidMass, float fillingLiquidTemperature) => _currentLiquidState.Fill(fillingLiquidMass, fillingLiquidTemperature);
    public void Drain(float drainingLiquidMass) => _currentLiquidState.Drain(drainingLiquidMass);

    public override void HeatUp(float amountOfHeat) => _currentLiquidState.HeatUp(amountOfHeat);
    public override void CoolDown(float amountOfHeat) => _currentLiquidState.CoolDown(amountOfHeat);

    public void Evaporate() => _currentLiquidState.Evaporate();

    protected override void TransferHeat(Heatable heatReceiver) => _currentLiquidState.TransferHeat(heatReceiver);
    protected override void TransferHeatToAir() => _currentLiquidState.TransferHeatToAir();    

    private float GetMassEvaporatingInHour(float temperature)
    {
        // Math function, which calculates evaporation
        float massEvaporatingInHour = ((0.0001f * Mathf.Pow(temperature, 3)) - (0.0032f * Mathf.Pow(temperature, 2)) + (0.1227f * temperature) - 0.8783f) / 3f;
        
        // For small values result can be negative, so it must be Clamped
        Mathf.Clamp(massEvaporatingInHour, 0, float.MaxValue);

        return massEvaporatingInHour;
    }

    public float GetMassEvaporatingInSecond(float temperature)
    {
        return GetMassEvaporatingInHour(temperature) / 60 / 60;
    }

    public BaseLiquidState GetLiquidState<T>() where T : BaseLiquidState
    {
        return _liquidStates.FirstOrDefault(s => s is T);
    }

    public void SwitchLiquidState<T>() where T : BaseLiquidState
    {
        BaseLiquidState state = _liquidStates.FirstOrDefault(s => s is T);

        if (state != _currentLiquidState)
        {
            _currentLiquidState?.Exit();                     
            state.Enter();
            _currentLiquidState = state;            
        }
    }
}