using UnityEngine;
using UniRx;

public abstract class Heatable : MonoBehaviour
{
    [Tooltip("Kilograms")]
    [SerializeField] protected ReactiveProperty<float> _observableMass = new ReactiveProperty<float>();
    [SerializeField] protected ReactiveProperty<float> _observableTemperature = new ReactiveProperty<float>();

    [Tooltip("Joules / (Kilogram * Kelvin)")]
    [SerializeField] private float _specificHeat;


    public IReadOnlyReactiveProperty<float> ObservableMass => _observableMass;
    public IReadOnlyReactiveProperty<float> ObservableTemperature => _observableTemperature;
   
    public float Mass
    {
        get => _observableMass.Value;

        protected set
        {
            if(value <= 0)
            {
                _observableMass.Value = 0;
            }
            else
            {
                _observableMass.Value = value;
            }
        }
    }

    public float Temperature
    {
        get => _observableTemperature.Value;

        protected set
        {
            _observableTemperature.Value = value;
        }
    }


    public float SpecificHeat => _specificHeat;
    

    public virtual void HeatUp(float amountOfHeat) => _observableTemperature.Value += amountOfHeat / (_specificHeat * _observableMass.Value);
    public virtual void CoolDown(float amountOfHeat) => _observableTemperature.Value -= amountOfHeat / (_specificHeat * _observableMass.Value);

    public float ConvertHeatToTemperature(float heat) => heat / (_observableMass.Value * _specificHeat);
    public float ConvertTemperatureToHeat(float temperature) => temperature * _observableMass.Value * _specificHeat;

    protected virtual void TransferHeat(Heatable heatReceiver)
    {
        float heat = Mathf.Pow(Mathf.Abs(_observableTemperature.Value - heatReceiver.Temperature), 3) * Time.deltaTime;        
        heatReceiver.HeatUp(heat);
        CoolDown(heat);        
    }
    protected abstract void TransferHeatToAir();
}