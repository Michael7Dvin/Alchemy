using System.Collections.Generic;
using UnityEngine;

public class PotionLiquid : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [SerializeField] private Liquid _liquid;
    private List<Ingredient> _ingredients = new();

    public string Name => _name;
    public string Description => _description;
    public Liquid LiquidBase => _liquid;
    public List<Ingredient> Ingredients => _ingredients;
}
