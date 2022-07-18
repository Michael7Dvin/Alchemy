using UnityEngine;
using UniRx;

public abstract class BaseLiquidState
{
    protected readonly ReactiveProperty<float> _mass;
    protected readonly ReactiveProperty<float> _temperature;

    protected Liquid _liquid;

    protected readonly CompositeDisposable _disposable = new CompositeDisposable();


    protected BaseLiquidState(Liquid liquid, ReactiveProperty<float> mass, ReactiveProperty<float> temperature)
    {
        _liquid = liquid;      
        _mass = mass;
        _temperature = temperature;
    }


    public virtual void Enter()
    {
        _mass.Subscribe(mass =>
        {
            HandleMassChange(mass);

        }).AddTo(_disposable);

        _temperature.Subscribe(temperature =>
        {
            HandleTemperatureChange(temperature);

        }).AddTo(_disposable);
    }

    public virtual void Exit()
    {
        _disposable.Clear();
    }

    public virtual void Fill(float fillingLiquidMass, float fillingLiquidTemperature)
    {
        _temperature.Value =
            (_liquid.Mass * _liquid.Temperature + fillingLiquidMass * fillingLiquidTemperature) /
            (_liquid.Mass + fillingLiquidMass);

        _mass.Value = _liquid.Mass + fillingLiquidMass;
    }

    public virtual void Drain(float drainingLiquidMass)
    {
        _mass.Value = _liquid.Mass - drainingLiquidMass;
    }

    public virtual void HeatUp(float amountOfHeat)
    {
        bool isIncomingHeatWillExceedBoilingTemperature = (_liquid.Temperature + _liquid.ConvertHeatToTemperature(amountOfHeat)) > _liquid.LiquidType.FinalBoilingTemperature;

        if (isIncomingHeatWillExceedBoilingTemperature)
        {
            float extraHeat = _liquid.ConvertTemperatureToHeat(_liquid.Temperature) + amountOfHeat - _liquid.ConvertTemperatureToHeat(_liquid.LiquidType.FinalBoilingTemperature);
            float extraTemperature = _liquid.ConvertHeatToTemperature(extraHeat);

            void BaseHeatUp(float amountOfHeat)
            {
                _temperature.Value += amountOfHeat / (_liquid.SpecificHeat * _liquid.Mass);
            }

            BaseHeatUp(amountOfHeat - extraHeat);

            float extraEvaporatingMass =
                _liquid.GetMassEvaporatingInSecond(_liquid.Temperature + extraTemperature) -
                _liquid.GetMassEvaporatingInSecond(_liquid.LiquidType.FinalBoilingTemperature);

            _mass.Value = _liquid.Mass - extraEvaporatingMass;
        }
        else
        {
            _temperature.Value = _liquid.Temperature + (amountOfHeat / (_liquid.SpecificHeat * _liquid.Mass));
        }
    }

    public virtual void CoolDown(float amountOfHeat)
    {
        _temperature.Value -= amountOfHeat / (_liquid.SpecificHeat * _liquid.Mass);
    }

    public virtual void TransferHeat(Heatable heatReceiver)
    {
        float heat = Mathf.Pow(Mathf.Abs(_liquid.Temperature - heatReceiver.Temperature), 3) * Time.deltaTime;
        heatReceiver.HeatUp(heat);
        CoolDown(heat);
    }

    public virtual void TransferHeatToAir()
    {
        float airTemperature = 25f;
        float heat;

        if (_liquid.Temperature < airTemperature)
        {
            heat = Mathf.Pow(Mathf.Abs(_liquid.Temperature - airTemperature), 2.5f) * Time.deltaTime;
            _liquid.HeatUp(heat);
        }
        else
        {
            _liquid.Evaporate();
        }
    }

    public virtual void Evaporate()
    {
        float massEvaporatingInSecond = _liquid.GetMassEvaporatingInSecond(_liquid.Temperature) * Time.deltaTime;

        float EvaporationHeat = _liquid.LiquidType.VaporizationEnthalpy * massEvaporatingInSecond;

        if (_liquid.Mass - massEvaporatingInSecond <= 0)
        {
            _mass.Value = 0f;
            _temperature.Value = 0f;
        }
        else
        {
            _mass.Value = _liquid.Mass - massEvaporatingInSecond;
            _temperature.Value = _liquid.Temperature - _liquid.ConvertHeatToTemperature(EvaporationHeat);
        }
    }

    protected virtual void HandleMassChange(float mass)
    {
        if (mass <= 0)
        {
            _liquid.SwitchLiquidState<NoneLiquidState>();
        }
    }

    protected abstract void HandleTemperatureChange(float temperature);      
}
