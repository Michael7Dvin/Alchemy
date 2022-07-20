using UnityEngine;

public class CauldronLiquidProcesses : Heatable
{
    [Tooltip("Percent of receiving heat transfering directly to liquid")]
    [SerializeField] private float _thermalConductivityFactor;

    [SerializeField] private Liquid _liquid;


    private void Update()
    {
        TransferHeatToAir();

        if (_liquid != null && _liquid.IsCurrentLiquidStateEqualsT<NoneLiquidState>() == false && Temperature > _liquid.Temperature)
        {
            TransferHeat(_liquid);
        }
    }
    

    public void FillLiquid(float fillingLiquidMass, float fillingLiquidTemperature)
    {
        _liquid.Fill(fillingLiquidMass, fillingLiquidTemperature);
    }

    public void DrainLiquid(float drainingLiquidMass) => _liquid.Drain(drainingLiquidMass);

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
}
