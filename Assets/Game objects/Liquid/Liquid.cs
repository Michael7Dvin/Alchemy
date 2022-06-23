using System;
using UnityEngine;

public class Liquid : Heatable
{
    [SerializeReference] private LiquidType _liquidType;
    [SerializeField] private Cauldron _cauldron;
    

    private void Update()
    {
        TransferHeatToAir();

        if (Temperature > _cauldron.Temperature)
        {
            TransferHeat(_cauldron);
        }
    }
    

    public event Action EvaporatedCompletely;
    
    
    public void Fill(float fillingLiquidMass, float fillingLiquidTemperature)
    {
        Temperature = 
            (Mass * Temperature + fillingLiquidMass * fillingLiquidTemperature) / 
            (Mass + fillingLiquidMass);

        Mass += fillingLiquidMass;        
    }

    public void Drain(float drainingLiquidMass) => Mass -= drainingLiquidMass;

    public override void HeatUp(float amountOfHeat)
    {
        bool isIncomingHeatWillExceedBoilingTemperature = (Temperature + ConvertHeatToTemperature(amountOfHeat)) > _liquidType.BoilingTemperature;

        if (isIncomingHeatWillExceedBoilingTemperature)
        {
            float extraHeat = ConvertTemperatureToHeat(Temperature) + amountOfHeat - ConvertTemperatureToHeat(_liquidType.BoilingTemperature);
            float extraTemperature = ConvertHeatToTemperature(extraHeat);
          
            base.HeatUp(amountOfHeat - extraHeat);

            float extraEvaporating = 
                GetMassEvaporatingInSecond(Temperature + extraTemperature / Time.deltaTime) - 
                GetMassEvaporatingInSecond(_liquidType.BoilingTemperature);
         
            Mass -= extraEvaporating;           
        }
        else
        {
            base.HeatUp(amountOfHeat);   
        }
    }

    protected override void TransferHeatToAir()
    {       
        float airTemperature = 25f;
        float heat;

        if (Temperature < airTemperature)
        {
            
            heat = Mathf.Pow(Mathf.Abs(Temperature - airTemperature), 2.5f) * Time.deltaTime;
            HeatUp(heat);
        }
        else
        {
            Evaporate();
        }
    }

    private float GetMassEvaporatingInHour(float temperature)
    {
        // Math function, which calculates evaporation
        float massEvaporatingInHour = ((0.0001f * Mathf.Pow(temperature, 3)) - (0.0032f * Mathf.Pow(temperature, 2)) + (0.1227f * temperature) - 0.8783f) / 3f;
        
        // For small values result can be negative, so it must be Clamped
        Mathf.Clamp(massEvaporatingInHour, 0, float.MaxValue);

        return massEvaporatingInHour;
    }

    private float GetMassEvaporatingInSecond(float temperature)
    {
        return GetMassEvaporatingInHour(temperature) / 60 / 60;
    }

    private void Evaporate()
    {
        float massEvaporatingInSecond = GetMassEvaporatingInSecond(Temperature) * Time.deltaTime;
        
        float EvaporationHeat = _liquidType.VaporizationEnthalpy * massEvaporatingInSecond;

        if (Mass - massEvaporatingInSecond <= 0)
        {
            EvaporatedCompletely?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Mass -= massEvaporatingInSecond;
        }
        Temperature -= ConvertHeatToTemperature(EvaporationHeat);
    }        
}