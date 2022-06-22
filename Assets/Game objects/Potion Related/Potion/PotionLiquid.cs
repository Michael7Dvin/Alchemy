using System.Collections.Generic;
using UnityEngine;

public class PotionLiquid : MonoBehaviour
{
    [SerializeField] private Liquid _liquid;
    private List<Ingredient> _ingredients = new List<Ingredient>();

    public Liquid LiquidBase => _liquid;
    public List<Ingredient> Ingredients => _ingredients;





   
}
