
using UniRx;

public class BoilingLiquidState : BaseLiquidState
{
    public BoilingLiquidState(Liquid liquid, ReactiveProperty<float> mass, ReactiveProperty<float> temperature) : base(liquid, mass, temperature)
    {
    }


    protected override void HandleTemperatureChange(float temperature)
    {
        if(temperature < _liquid.LiquidType.InitialBoilingTemperature)
        {
            _liquid.SwitchLiquidState<NormalLiquidState>();
        }
    }
}
