using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Potion : Liquid
{
    [SerializeField] private List<Ingredient> _ingredients = new List<Ingredient>();
    [SerializeField] private Dictionary<MagicElement, int> _exccessMagicElements = new Dictionary<MagicElement, int>();

    private PotionRecipesDataBase _potionRecipesDataBase;


    private List<PotionRecipe> CorrespondingRecipes => _potionRecipesDataBase.GetCorrespondingRecipes(AllMagicElements);
    
    private Dictionary<MagicElement, int> AllMagicElements
    {
        get
        {
            Dictionary<MagicElement, int> elements = new Dictionary<MagicElement, int>();

            foreach (Ingredient ingredient in _ingredients)
            {
                foreach (KeyValuePair<MagicElement, int> ingredientMagicElement in ingredient.MagicElements.Elements)
                {
                    if (elements.ContainsKey(ingredientMagicElement.Key))
                    {
                        elements[ingredientMagicElement.Key] += ingredientMagicElement.Value;
                    }
                    else
                    {
                        elements.Add(ingredientMagicElement.Key, ingredientMagicElement.Value);
                    }
                }
            }

            return elements;
        }        
    }
    private Dictionary<MagicElement, int> ExccessMagicElements
    {
        get
        {
            if(CorrespondingRecipes.Count == 0 | CorrespondingRecipes.Count > 1)
            {
                Debug.LogError("Unavailable to get ExccessMagicElements");
                return null;
            }


            Dictionary<MagicElement, int> exccessElements = new Dictionary<MagicElement, int>();

            foreach (var element in AllMagicElements)
            {
                if (CorrespondingRecipes[0].Elements.ContainsKey(element.Key) == true)
                {
                    int exccess = element.Value - (CorrespondingRecipes[0].Elements[element.Key] * PotionAmount);

                    if (exccess > 0)
                    {
                        exccessElements.Add(element.Key, exccess);
                    }
                }
                else
                {
                    exccessElements.Add(element.Key, element.Value);
                }
            }

            return exccessElements;
        }
    }

    private int PotionAmount
    {
        get 
        {
            if (CorrespondingRecipes.Count == 0 | CorrespondingRecipes.Count > 1)
            {
                Debug.LogError("Unavailable to get PotionAmount");
                return 0;
            }


            List<int> potionAmounts = new List<int>();

            foreach (var element in CorrespondingRecipes[0].Elements)
            {
                float quotient = AllMagicElements[element.Key] / element.Value;

                int amount = Convert.ToInt32(Mathf.Floor(quotient));

                potionAmounts.Add(amount);
            }

            return potionAmounts.Min();        
        }
    }


    [Inject]
    private void Construct(PotionRecipesDataBase potionRecipesDataBase)
    {
        _potionRecipesDataBase = potionRecipesDataBase;
    }


    public void AddIngredient(Ingredient ingredient)
    {
        _ingredients.Add(ingredient);
    }
}
