using System.Collections.Generic;
using UnityEngine;

public class PotionLiquid : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [SerializeField] private PotionLiquidBase _liquidBase;
    private List<Ingredient> _ingredients = new();

    public string Name => _name;
    public string Description => _description;
    public PotionLiquidBase LiquidBase => _liquidBase;
    public List<Ingredient> Ingredients => _ingredients;


}
