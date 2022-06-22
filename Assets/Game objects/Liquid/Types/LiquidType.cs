using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Liquid Type")]
public class LiquidType : ScriptableObject
{
    [SerializeField] private float _boilingTemperature;
    [SerializeField, Tooltip("Joules / Kilogram")] private float _vaporizationEnthalpy;

    public float BoilingTemperature => _boilingTemperature;
    public float VaporizationEnthalpy => _vaporizationEnthalpy;
}
