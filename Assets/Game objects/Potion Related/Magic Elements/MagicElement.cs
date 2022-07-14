using UnityEngine;

public enum MagicElements
{
    None = 0,
    Life = 1,
    Earth = 2,
    Blood = 3,
    Electricity = 4,
    Defence = 5,
    Speed = 6,
    Poison = 7,
    Death = 8,
    Lightness = 9,
    Light = 10
}

[CreateAssetMenu(menuName = "Scriptable Objects/Magic Element")]
public class MagicElement : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private MagicElements _magicElementType;
    [SerializeField] private int _magicElementStrength;

    public string Name => _name;
    public string Description => _description;
    public MagicElements Element => _magicElementType;
    public int ElementStrength => _magicElementStrength;
}
