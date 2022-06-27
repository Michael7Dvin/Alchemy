using UnityEngine;

public enum MagicElement
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

[CreateAssetMenu(menuName = "Scriptable Objects/Ingredient Magic Element")]
public class IngredientMagicElement : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private MagicElement _element;
    [SerializeField] private int _elementStrength;

    public string Name => _name;
    public string Description => _description;
    public MagicElement Element => _element;
    public int ElementStrength => _elementStrength;
}
