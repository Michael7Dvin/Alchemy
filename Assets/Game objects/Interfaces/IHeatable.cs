using UnityEngine;

public interface IHeatable
{
    //Kg
    public float Mass { get; set; }
    //°C
    public float Temperature { get; set; }
    // Joules / (Kilogram * Kelvin)
    public float SpecificHeat { get; }


    public float ConvertHeatToTemperature(float heat) => heat / (Mass * SpecificHeat);
    public float ConvertTemperatureToHeat(float temperature) => temperature * Mass * SpecificHeat;

    public void HeatUp(float amountOfHeat) => Temperature += amountOfHeat / (SpecificHeat * Mass);
    public void CoolDown(float amountOfHeat) => Temperature -= amountOfHeat / (SpecificHeat * Mass);

    public void TransferHeat(IHeatable heatReceiver)
    {
        float heat = Mathf.Pow(Mathf.Abs(Temperature - heatReceiver.Temperature), 3) * PhysicalProcessesSimulation.SpeedCorrection;        
        heatReceiver.HeatUp(heat);
        CoolDown(heat);
    }

    public void TransferHeatToAir();
}