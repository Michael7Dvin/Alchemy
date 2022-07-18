using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Liquid Type")]
public class LiquidType : ScriptableObject
{
    [SerializeField] private float _initialBoilingTemperature;
    [SerializeField] private float _finalBoilingTemperature;
    [SerializeField, Tooltip("Joules / Kilogram")] private float _vaporizationEnthalpy;

    public float InitialBoilingTemperature => _initialBoilingTemperature;
    public float FinalBoilingTemperature => _finalBoilingTemperature;
    public float VaporizationEnthalpy => _vaporizationEnthalpy;
}
