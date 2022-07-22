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
        List<MagicElement> unoccupiedMagicElements = potionMagicElements.FindAll(x => x);

        return recipe.MagicElements.All(recipeElement =>
        {
            List<MagicElement> allCorrespondingMagicElements = unoccupiedMagicElements
            .FindAll(potionElement => potionElement.Type == recipeElement.Type && potionElement.Strength >= recipeElement.Strength);

            if (allCorrespondingMagicElements.Count == 0)
            {
                return false;
            }

            int minimumStrength = allCorrespondingMagicElements.Min(x => x.Strength);
            MagicElement correspondMagicElement = allCorrespondingMagicElements.FirstOrDefault(x => x.Strength == minimumStrength);

            unoccupiedMagicElements.Remove(correspondMagicElement);
            return true;
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
