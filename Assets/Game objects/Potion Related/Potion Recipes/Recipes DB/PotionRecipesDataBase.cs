using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class PotionRecipesDataBase : MonoBehaviour
{
    [SerializeField] private List<PotionRecipe> _potionRecipesToAdd = new List<PotionRecipe>();
    private List<PotionRecipe> _potionRecipes = new List<PotionRecipe>();


    public List<PotionRecipe> PotionRecipes => _potionRecipes;


    private void Awake()
    {
        foreach (var recipe in _potionRecipesToAdd)
        {            
            AddRecipe(recipe);
        }
    }


    public List<PotionRecipe> GetCorrespondingRecipes(List<MagicElement> potionMagicElements)
    {
        return _potionRecipes.Where(recipe => IsCorrespondsRecipe(potionMagicElements, recipe)).ToList();
    }

    private bool IsCorrespondsRecipe(List<MagicElement> potionMagicElements, PotionRecipe recipe)
    {
        return recipe.MagicElements.All(x =>
        {
            if (potionMagicElements.Contains(x))
            {
                return potionMagicElements.Count(y => y == x) >= recipe.MagicElements.Count(y => y == x);
            }
            return false;
        });
    }

    private void AddRecipe(PotionRecipe potionRecipe)
    {
        if(_potionRecipes.Contains(potionRecipe))
        {
            Debug.LogWarning($"PotionRecipesDataBase already contains [{potionRecipe}], remove all repeating [{potionRecipe}] from PotionRecipesToAdd");
        }
        else if(potionRecipe.MagicElements.Count > 0)
        {
            _potionRecipes.Add(potionRecipe);
        }
    }

}
