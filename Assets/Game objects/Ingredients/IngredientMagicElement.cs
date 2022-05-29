using UnityEngine;

public enum MagicElement
{
    None = 0,
    Life = 1,
    Earth = 2,
    Blood = 3,
    Electricity = 4,
    Durability = 5,
    Speed = 6,
}

[CreateAssetMenu(menuName = "Scriptable Objects/Ingredient Magic Element")]
public class IngredientMagicElement : ScriptableObject
{
    [SerializeField] private readonly string _name;
    [SerializeField] private string _description;
    [SerializeField] private MagicElement _element;
    [SerializeField] private float _elementStrength;

    public string Name => _name;
    public string Description => _description;
    public MagicElement Element => _element;
    public float ElementStrength => _elementStrength;
}
