using UniRx;
using UnityEngine;

public class NoneLiquidState : BaseLiquidState
{
    public NoneLiquidState(Liquid liquid, ReactiveProperty<float> mass, ReactiveProperty<float> temperature) : base(liquid, mass, temperature)
    {
    }


    public override void Enter()
    {
        base.Enter();

        _mass.Value = 0;
        _temperature.Value = 0;
    }
    
    public override void Fill(float fillingLiquidMass, float fillingLiquidTemperature)
    {
        _mass.Value = fillingLiquidMass;
        _temperature.Value = fillingLiquidTemperature;
    }
    public override void Drain(float drainingLiquidMass) { }
           
    public override void HeatUp(float amountOfHeat) { }
    public override void CoolDown(float amountOfHeat) { }
    
    public override void Evaporate() { }

    public override void TransferHeat(Heatable heatReceiver) { }
    public override void TransferHeatToAir() { }

    protected override void HandleMassChange(float mass)
    {
        if (mass > 0)
        {
            _liquid.SwitchLiquidState<NormalLiquidState>();
        }
    }

    protected override void HandleTemperatureChange(float temperature) { }
 
}
