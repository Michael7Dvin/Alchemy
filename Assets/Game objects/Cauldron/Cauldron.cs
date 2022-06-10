using System.Collections;
using UnityEngine;
using Zenject;

public class Cauldron : MonoBehaviour, IHeatable, IHaveRealTimePhysicalProcesses
{
    [SerializeField, Tooltip("Kilograms")] private float _mass;
    [SerializeField] private float _temperature;
    [SerializeField, Tooltip("Joules / (Kilogram * Kelvin)")] private float _specificHeat;
    [SerializeField, Tooltip("Percent of receiving heat transfering directly to liquid")] private float _thermalConductivityFactor;

    private Liquid _liquid;
    private IHeatable _self;


    public float Mass { get => _mass; set => _mass = value; }
    public float Temperature { get => _temperature; set => _temperature = value; }
    public float SpecificHeat { get => _specificHeat; }


    [Inject]
    private void Construct(Liquid liquid)
    {
        AddLiquid(liquid);
    }

    private void Awake()
    {
        _self = this;
    }

    private void Start()
    {
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }

    private void OnDisable()
    {
        if(_liquid != null)
            _liquid.EvaporateCompletely -= RemoveLiquid;
    }

    public IEnumerator ImplementRealTimePhysicalProcesses()
    {
        _self.TransferHeatToAir();
        if (_liquid != null && _temperature > _liquid.Temperature)
        {
            
            _self.TransferHeat(_liquid);
        }
                
        yield return new WaitForSeconds(PhysicalProcessesSimulation.SpeedCorrection);
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }

    public void HeatUp(float amountOfHeat)
    {        
        if (_liquid != null && _liquid.Temperature < _temperature)
        {
            float heatTransferingDirectlyToLiquid = amountOfHeat * _thermalConductivityFactor;
            float heatToSelf = amountOfHeat - heatTransferingDirectlyToLiquid;

            _temperature += _self.ConvertHeatToTemperature(heatToSelf);
            _liquid.HeatUp(heatTransferingDirectlyToLiquid);
        }
        else
        {
            _temperature += _self.ConvertHeatToTemperature(amountOfHeat);
        }
    }

    public void TransferHeatToAir()
    {
        float airTemperature = 25f;
        float amountOfHeat;

        if (_temperature > airTemperature)
        {
            amountOfHeat = Mathf.Pow(Mathf.Abs(_temperature - airTemperature), 2.5f) * PhysicalProcessesSimulation.SpeedCorrection;
            _self.CoolDown(amountOfHeat);
        }
        else
        {
            amountOfHeat = Mathf.Pow(Mathf.Abs(_temperature - airTemperature), 2.5f) * PhysicalProcessesSimulation.SpeedCorrection;
            HeatUp(amountOfHeat);
        }
    }

    private void AddLiquid(Liquid liquid)
    {
        if (_liquid == null)
        {
            _liquid = liquid;
            _liquid.EvaporateCompletely += RemoveLiquid;
        }
    }

    private void RemoveLiquid()
    {
        _liquid.EvaporateCompletely -= RemoveLiquid;
        _liquid = null;
    }
}
