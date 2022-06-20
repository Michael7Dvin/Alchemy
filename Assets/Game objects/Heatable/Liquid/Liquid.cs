using System;
using UnityEngine;

public class Liquid : Heatable
{
    [SerializeField] private float _boilingTemperature;
    [SerializeField, Tooltip("Joules / Kilogram")] private float _vaporizationEnthalpy;

    [SerializeField] private Cauldron _container;
    

    public event Action EvaporatedCompletely;


    private void Update()
    {
        TransferHeatToAir();

        if (Temperature > _container.Temperature)
        {
            TransferHeat(_container);
        }
    }
    

    public override void HeatUp(float amountOfHeat)
    {
        bool isIncomingHeatWillExceedBoilingTemperature = (Temperature + ConvertHeatToTemperature(amountOfHeat)) > _boilingTemperature;

        if (isIncomingHeatWillExceedBoilingTemperature)
        {
            float extraHeat = ConvertTemperatureToHeat(Temperature) + amountOfHeat - ConvertTemperatureToHeat(_boilingTemperature);
            float extraTemperature = ConvertHeatToTemperature(extraHeat);
          
            base.HeatUp(amountOfHeat - extraHeat);

            float extraEvaporating = 
                GetMassEvaporatingInSecond(Temperature + extraTemperature / Time.deltaTime) - 
                GetMassEvaporatingInSecond(_boilingTemperature);
         
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
        
        float EvaporationHeat = _vaporizationEnthalpy * massEvaporatingInSecond;

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