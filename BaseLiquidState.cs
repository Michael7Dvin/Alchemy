using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLiquidState
{
    protected virtual void Fill(float fillingLiquidMass, float fillingLiquidTemperature)
    {

    }

    protected virtual void Drain(float drainingLiquidMass)
    {

    }

    protected virtual void HeatUp(float amountOfHeat)
    {

    }

    protected virtual void TransferHeatToAir()
    {

    }

    protected virtual void Evaporate()
    {

    }
}
