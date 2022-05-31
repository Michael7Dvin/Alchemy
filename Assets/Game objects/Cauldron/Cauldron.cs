using UnityEngine;

public abstract class Cauldron : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [Tooltip("Kilograms")]
    [SerializeField] private float _mass;

    [Tooltip("Megajoule / Kilograms * Kelvin")]
    [SerializeField] private float _specificHeat;

    [SerializeField] private float _temperature;
    [SerializeField] private PotionLiquid _potionLiquid;     

    public string Name => _name;
    public string Description => _description;
    public float Mass => _mass;
    public float SpecificHeat => _specificHeat;
    public PotionLiquid PotionLiquid => _potionLiquid;

    private void AddPotionLiquidBase(PotionLiquidBase potionLiquidBase)
    {

    }

    private void AddIngredient(Ingredient ingredient)
    {

    }

    private void PourOutLiquid()
    {

    }
}
