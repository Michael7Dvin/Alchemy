using System.Collections.Generic;
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
        if(potionRecipe.IsVaild == true)
        {
            _potionRecipes.Add(potionRecipe);
        }
    }    
}
