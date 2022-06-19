using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class Liquid : Heatable, IHaveRealTimePhysicalProcesses
{
    [SerializeField] private float _boilingTemperature;
    [SerializeField, Tooltip("Joules / Kilogram")] private float _vaporizationEnthalpy;

    private Heatable _container;


    [Inject]
    private void Construct(Cauldron container)
    {
        _container = container;
    }

    private void Start()
    {
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }


    public event Action EvaporatedCompletely;


    public IEnumerator ImplementRealTimePhysicalProcesses()
    {
        TransferHeatToAir();

        if (Temperature > _container.Temperature)
        {
            TransferHeat(_container);
        }

        yield return new WaitForSeconds(PhysicalProcessesSimulation.SpeedCorrection);
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }

    public override void HeatUp(float amountOfHeat)
    {
        bool isIncomingHeatWillExceedBoilingTemperature = (Temperature + ConvertHeatToTemperature(amountOfHeat)) > _boilingTemperature;

        if (isIncomingHeatWillExceedBoilingTemperature)
        {
            float extraHeat = ConvertTemperatureToHeat(Temperature) + amountOfHeat - ConvertTemperatureToHeat(_boilingTemperature);
            float extraTemperature = ConvertHeatToTemperature(extraHeat);
          
            base.HeatUp(amountOfHeat - extraHeat);
            Debug.Log(amountOfHeat - extraHeat);

            float extraEvaporating = 
                GetMassEvaporatingInSecond(Temperature + extraTemperature / PhysicalProcessesSimulation.SpeedCorrection) - 
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
            
            heat = Mathf.Pow(Mathf.Abs(Temperature - airTemperature), 2.5f) * PhysicalProcessesSimulation.SpeedCorrection;
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
        float correctedMassEvaporatingInSecond;
        correctedMassEvaporatingInSecond = GetMassEvaporatingInSecond(Temperature) * PhysicalProcessesSimulation.SpeedCorrection;
        
        float EvaporationHeat = _vaporizationEnthalpy * correctedMassEvaporatingInSecond;

        if (Mass - correctedMassEvaporatingInSecond <= 0)
        {
            EvaporatedCompletely?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Mass -= correctedMassEvaporatingInSecond;
        }
        Temperature -= ConvertHeatToTemperature(EvaporationHeat);
    }    
}