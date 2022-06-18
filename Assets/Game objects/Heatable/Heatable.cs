using UnityEngine;

public abstract class Heatable : MonoBehaviour
{
    [Tooltip("Kilograms")]
    [SerializeField] private float _mass;
    [SerializeField] private float _temperature;
    [Tooltip("Joules / (Kilogram * Kelvin)")]
    [SerializeField] private float _specificHeat;


    public float Temperature { get => _temperature; protected set => _temperature = value; }
    protected float Mass { get => _mass; set => _mass = value; }
    

    public virtual void HeatUp(float amountOfHeat) => Temperature += amountOfHeat / (_specificHeat * _mass);
    public virtual void CoolDown(float amountOfHeat) => Temperature -= amountOfHeat / (_specificHeat * _mass);

    protected void TransferHeat(Heatable heatReceiver)
    {
        float heat = Mathf.Pow(Mathf.Abs(Temperature - heatReceiver.Temperature), 3) * PhysicalProcessesSimulation.SpeedCorrection;        
        heatReceiver.HeatUp(heat);
        CoolDown(heat);
    }

    protected float ConvertHeatToTemperature(float heat) => heat / (_mass * _specificHeat);
    protected float ConvertTemperatureToHeat(float temperature) => temperature * _mass * _specificHeat;

    protected abstract void TransferHeatToAir();
}