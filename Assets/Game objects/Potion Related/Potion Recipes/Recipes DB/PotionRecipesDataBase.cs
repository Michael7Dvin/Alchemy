using System.Collections.Generic;
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


    private void AddRecipe(PotionRecipe potionRecipe)
    {
        if(_potionRecipes.Contains(potionRecipe))
        {
            Debug.LogWarning($"PotionRecipesDataBase already contains [{potionRecipe}], remove all repeating [{potionRecipe}] from PotionRecipesToAdd");
        }
        else if(potionRecipe.IsVaild == true)
        {
            _potionRecipes.Add(potionRecipe);
        }
    }

    private bool IsCorrespondsRecipe(ReactiveDictionary<MagicElement, int> potionElements, Dictionary<MagicElement, int> recipe)
    {
        foreach (var item in recipe)
        {
            if (potionElements.ContainsKey(item.Key))
            {
                if (potionElements[item.Key] < item.Value)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public List<PotionRecipe> GetCorrespondingRecipes(ReactiveDictionary<MagicElement, int> potionMagicElements)
    {
        List<PotionRecipe> correspondingRecipes = new List<PotionRecipe>();

        foreach (var recipe in _potionRecipes)
        {
            if (IsCorrespondsRecipe(potionMagicElements, recipe.Elements) == true)
            {
                correspondingRecipes.Add(recipe);
            }
        }

        return correspondingRecipes;
    }
}
