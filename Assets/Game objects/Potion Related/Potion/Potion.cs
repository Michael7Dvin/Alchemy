
using System.Collections.Generic;
using UnityEngine;

public class Potion : Liquid
{
    [SerializeField] private List<Ingredient> _ingredients = new List<Ingredient>();

    public List<Ingredient> Ingredients => _ingredients;




    public void AddIngredient(Ingredient ingredient)
    {
        _ingredients.Add(ingredient);
    }




}
