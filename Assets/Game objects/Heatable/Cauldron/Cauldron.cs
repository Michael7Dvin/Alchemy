using System.Collections;
using UnityEngine;
using Zenject;

public class Cauldron : Heatable, IHaveRealTimePhysicalProcesses
{
    [Tooltip("Percent of receiving heat transfering directly to liquid")]
    [SerializeField] private float _thermalConductivityFactor;
    private Liquid _liquid;


    [Inject]
    private void Construct(Liquid liquid)
    {
        AddLiquid(liquid);
    }

    private void Start()
    {
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }

    private void OnDisable()
    {
        if(_liquid != null)
        {
            _liquid.EvaporatedCompletely -= RemoveLiquid;
        }
    }


    public IEnumerator ImplementRealTimePhysicalProcesses()
    {
        TransferHeatToAir();
        if (_liquid != null && Temperature > _liquid.Temperature)
        {            
            TransferHeat(_liquid);
        }
                
        yield return new WaitForSeconds(PhysicalProcessesSimulation.SpeedCorrection);
        StartCoroutine(ImplementRealTimePhysicalProcesses());
    }

    public override void HeatUp(float amountOfHeat)
    {        
        if (_liquid != null && _liquid.Temperature < Temperature)
        {
            float heatTransferingDirectlyToLiquid = amountOfHeat * _thermalConductivityFactor;
            float heatToSelf = amountOfHeat - heatTransferingDirectlyToLiquid;

            Temperature += ConvertHeatToTemperature(heatToSelf);
            _liquid.HeatUp(heatTransferingDirectlyToLiquid);
        }
        else
        {
            Temperature += ConvertHeatToTemperature(amountOfHeat);
        }
    }

    protected override void TransferHeatToAir()
    {
        float airTemperature = 25f;
        float amountOfHeat;

        if (Temperature > airTemperature)
        {
            amountOfHeat = Mathf.Pow(Mathf.Abs(Temperature - airTemperature), 2.5f) * PhysicalProcessesSimulation.SpeedCorrection;
            CoolDown(amountOfHeat);
        }
        else
        {
            amountOfHeat = Mathf.Pow(Mathf.Abs(Temperature - airTemperature), 2.5f) * PhysicalProcessesSimulation.SpeedCorrection;
            HeatUp(amountOfHeat);
        }
    }
    

    private void AddLiquid(Liquid liquid)
    {
        if (_liquid == null)
        {
            _liquid = liquid;
            _liquid.EvaporatedCompletely += RemoveLiquid;
        }
    }

    private void RemoveLiquid()
    {
        _liquid.EvaporatedCompletely -= RemoveLiquid;
        _liquid = null;
    }
}
