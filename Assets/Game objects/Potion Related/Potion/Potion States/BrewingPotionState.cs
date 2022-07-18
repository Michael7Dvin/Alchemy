using System.Collections.Generic;
using UniRx;

public class BrewingPotionState : BasePotionState
{
    private List<PotionRecipe> _previousCorrespondingRecipes;


    public BrewingPotionState(Potion potion, ReactiveDictionary<MagicElement, int> magicElements) : base(potion, magicElements)
    {
    }


    public override void AddIngredient(Ingredient ingredient)
    {
        _previousCorrespondingRecipes = _potion.CorrespondingRecipes;

        foreach (KeyValuePair<MagicElement, int> ingredientMagicElement in ingredient.MagicElements.Elements)
        {
            if (_magicElements.ContainsKey(ingredientMagicElement.Key))
            {
                _magicElements[ingredientMagicElement.Key] += ingredientMagicElement.Value;
            }
            else
            {
                _magicElements.Add(ingredientMagicElement.Key, ingredientMagicElement.Value);
            }
        }
    }

    protected override void HandleMagicElementsChange()
    {
        if (_potion.CorrespondingRecipes.Count == 0)
        {
            _potion.SwitchPotionState<NotCorrespondToAnyRecipePotionState>();
        }
        else if (_potion.CorrespondingRecipes.Count == 1)
        {
            if(_previousCorrespondingRecipes[0] == _potion.CorrespondingRecipes[0])
            {
                return;
            }

            _potion.SwitchPotionState<CorrespondToSingleRecipePotionState>();
        }
        else if (_potion.CorrespondingRecipes.Count > 1)
        {
            _potion.SwitchPotionState<CorrespondToSeveralRecipesPotionState>();
        }
    }
}
