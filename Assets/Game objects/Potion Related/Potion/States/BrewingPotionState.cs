using System.Collections.Generic;
using UniRx;

public class BrewingPotionState : BasePotionState
{
    private List<PotionRecipe> _previousCorrespondingRecipes;

    public BrewingPotionState(Potion potion, PotionMagicElements potionMagicElements) : base(potion, potionMagicElements)
    {
    }


    public override void AddIngredient(Ingredient ingredient)
    {
        _previousCorrespondingRecipes = _potion.CorrespondingRecipes;

        base.AddIngredient(ingredient);
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
