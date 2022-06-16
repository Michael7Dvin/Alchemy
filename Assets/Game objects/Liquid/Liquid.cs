using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class Liquid : MonoBehaviour, IHeatable, IHaveRealTimePhysicalProcesses
{
    [SerializeField, Tooltip("Millilitres")] private float _volume;
    [SerializeField] private float _boilingTemperature;

    [SerializeField] private float _temperature;
    [SerializeField, Tooltip("Joules / Kilogram * Kelvin")] private float _specificHeat;
    [SerializeField, Tooltip("Joules / Kilogram")] private float _vaporizationEnthalpy;

    [SerializeField] private IHeatable _container;

    private IHeatable _self;

    public event Action EvaporateCompletely;

    public float Mass { get => _volume / 1000; set => _volume = value * 1000; }
    public float Temperature { get => _temperature; set => _temperature = value; }
    public float SpecificHeat => _specificHeat;

    [Inject]
    private void Construct(Cauldron container)
    {
        _container = container;
    }

    private void Awake()
    {
        _self = this;        
    }

    private void Start()
    {
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }

    public IEnumerator ImplementRealTimePhysicalProcesses()
    {
        TransferHeatToAir();

        if (Temperature > _container.Temperature)
        {
            _self.TransferHeat(_container);
        }

        yield return new WaitForSeconds(PhysicalProcessesSimulation.SpeedCorrection);
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }

    private float GetMassEvaporatingInHour(float temperature)
    {
        //math function, which calculates evaporation
        return ((0.0001f * Mathf.Pow(temperature, 3)) - (0.0032f * Mathf.Pow(temperature, 2)) + (0.1227f * temperature) - 0.8783f) / 3f;
    }

    private float GetMassEvaporatingInSecond(float temperature)
    {
        return GetMassEvaporatingInHour(temperature) / 60 / 60;
    }
    public void HeatUp(float amountOfHeat)
    {
        bool isIncomingHeatWillExceedBoilingTemperature = (_temperature + _self.ConvertHeatToTemperature(amountOfHeat)) > _boilingTemperature;

        if (isIncomingHeatWillExceedBoilingTemperature)
        {
            float extraHeat = _self.ConvertTemperatureToHeat(_temperature) + amountOfHeat - _self.ConvertTemperatureToHeat(_boilingTemperature);

            Temperature += _self.ConvertHeatToTemperature(amountOfHeat) - _self.ConvertHeatToTemperature(extraHeat);

            float extraTemperature = _self.ConvertHeatToTemperature(extraHeat);

            float extraEvaporating = 
                GetMassEvaporatingInSecond(Temperature + extraTemperature / PhysicalProcessesSimulation.SpeedCorrection) - 
                GetMassEvaporatingInSecond(_boilingTemperature);
         
            Mass -= extraEvaporating;
            
        }
        else
        {
            Temperature += amountOfHeat / (_specificHeat * Mass);   
        }
    }
    public void TransferHeatToAir()
    {       
        float airTemperature = 25f;
        float heat;

        if (_temperature < airTemperature)
        {
            
            heat = Mathf.Pow(Mathf.Abs(Temperature - airTemperature), 2.5f) * PhysicalProcessesSimulation.SpeedCorrection;
            HeatUp(heat);
        }
        else
        {
            Evaporate();
        }
    }
    
    private void Evaporate()
    {
        float correctedMassEvaporatingInSecond;

        //evaporating cannot be negative
        if (GetMassEvaporatingInSecond(_temperature) < 0)
        {
            correctedMassEvaporatingInSecond = 0f;
        }
        else
        {
            correctedMassEvaporatingInSecond = GetMassEvaporatingInSecond(_temperature) * PhysicalProcessesSimulation.SpeedCorrection;
        }

        float EvaporationHeat = _vaporizationEnthalpy * correctedMassEvaporatingInSecond;

        if (Mass - correctedMassEvaporatingInSecond <= 0)
        {
            EvaporateCompletely?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Mass -= correctedMassEvaporatingInSecond;
        }
        Temperature -= _self.ConvertHeatToTemperature(EvaporationHeat);
    }    
}