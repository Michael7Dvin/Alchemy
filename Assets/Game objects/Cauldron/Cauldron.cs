using System.Collections;
using UnityEngine;

public class Cauldron : MonoBehaviour, IHeatable, IHaveRealTimePhysicalProcesses
{
    [SerializeField, Tooltip("Kilograms")] private float _mass;
    [SerializeField] private float _temperature;
    [SerializeField, Tooltip("Joules / (Kilogram * Kelvin)")] private float _specificHeat;
    [SerializeField, Tooltip("Percent of receiving heat transfering directly to liquid")] private float _thermalConductivityFactor;

    private Liquid _liquid;

    private IHeatable _self;
    [SerializeField] private PotionLiquid _potionLiquid;

    //temp
    [SerializeField] private Liquid liquidToAdd;

    public float Mass { get => _mass; set => _mass = value; }
    public float Temperature { get => _temperature; set => _temperature = value; }
    public float SpecificHeat { get => _specificHeat; }


    //Everything should be in Start(), but it's in Awake() because of NullReferenceException error
    private void Awake() => _self = this;

    private void Start()
    {
        //temp
        if (liquidToAdd != null)
            AddLiquid(liquidToAdd);
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }

    private void OnDisable()
    {
        if(liquidToAdd != null)
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
            float heatTransferedDirectlyToLiquid = amountOfHeat * _thermalConductivityFactor;
            float heatToSelf = amountOfHeat - heatTransferedDirectlyToLiquid;

            _temperature += _self.ConvertHeatToTemperature(heatToSelf);
            _liquid.HeatUp(heatTransferedDirectlyToLiquid);
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
