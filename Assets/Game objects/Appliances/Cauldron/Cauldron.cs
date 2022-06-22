using UnityEngine;

public class Cauldron : Heatable
{
    [Tooltip("Percent of receiving heat transfering directly to liquid")]
    [SerializeField] private float _thermalConductivityFactor;
    
    private Liquid _liquid;
    [SerializeField] private Liquid _liquidToAdd;


    private void Start()
    {
        AddLiquid(_liquidToAdd);
    }

    private void OnDisable()
    {
        if(_liquid != null)
        {
            _liquid.EvaporatedCompletely -= RemoveLiquid;
        }
    }

    private void Update()
    {
        TransferHeatToAir();

        if (_liquid != null && Temperature > _liquid.Temperature)
        {
            TransferHeat(_liquid);
        }
    }
    

    public void FillLiquid(float fillingLiquidMass, float fillingLiquidTemperature)
    {
        _liquid.Fill(fillingLiquidMass, fillingLiquidTemperature);
    }

    public override void HeatUp(float amountOfHeat)
    {        
        if (_liquid != null && _liquid.Temperature < Temperature)
        {
            float heatTransferingDirectlyToLiquid = amountOfHeat * _thermalConductivityFactor;
            float heatToSelf = amountOfHeat - heatTransferingDirectlyToLiquid;

            base.HeatUp(heatToSelf);
            _liquid.HeatUp(heatTransferingDirectlyToLiquid);
        }
        else
        {
            base.HeatUp(amountOfHeat);          
        }
    }

    protected override void TransferHeatToAir()
    {
        float airTemperature = 25f;
        float amountOfHeat;

        if (Temperature > airTemperature)
        {
            amountOfHeat = Mathf.Pow(Mathf.Abs(Temperature - airTemperature), 2.5f) * Time.deltaTime;
            CoolDown(amountOfHeat);
        }
        else
        {
            amountOfHeat = Mathf.Pow(Mathf.Abs(Temperature - airTemperature), 2.5f) * Time.deltaTime;
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
