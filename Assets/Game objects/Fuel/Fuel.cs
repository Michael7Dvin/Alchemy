using UnityEngine;

public class Fuel : MonoBehaviour
{    
    [SerializeField, Tooltip("Kilograms")] private float _mass;
    [SerializeField, Tooltip("Megajoules / Kilogram")] private float _heatingValue;
    [SerializeField, Tooltip("Mass(Kg) burning in one second")] private float _massBurningInSecond;

    public delegate void OnBurnOut(Fuel fuel);
    public event OnBurnOut BurnOut;
    
    public float Mass => _mass;
    private float HeatingValueInJoulesPerKg => _heatingValue * 1000000;

    public float Burn()
    {
        float amountOfHeat = HeatingValueInJoulesPerKg * _massBurningInSecond * PhysicalProcessesSimulation.SpeedCorrection;

        if (_mass - _massBurningInSecond <= 0)
        {
            BurnOut?.Invoke(this);
            Destroy(gameObject);
            return amountOfHeat / (_massBurningInSecond/Mass); 
        }
        else 
        { 
            _mass -= _massBurningInSecond * PhysicalProcessesSimulation.SpeedCorrection;          
            return amountOfHeat;        
        }
    }
}
