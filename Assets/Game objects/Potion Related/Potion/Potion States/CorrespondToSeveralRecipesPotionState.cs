using UniRx;

public class CorrespondToSeveralRecipesPotionState : BasePotionState
{
    public CorrespondToSeveralRecipesPotionState(Potion potion, ReactiveDictionary<MagicElement, int> magicElements) : base(potion, magicElements)
    {
    }


    protected override void HandleMagicElementsChange()
    {
        if (_potion.CorrespondingRecipes.Count == 0)
        {
            _potion.SwitchPotionState<NotCorrespondToAnyRecipePotionState>();
        }
        else if (_potion.CorrespondingRecipes.Count == 1)
        {
            _potion.SwitchPotionState<CorrespondToSingleRecipePotionState>();
        }
        else if (_potion.CorrespondingRecipes.Count > 1)
        {
            return; 
        }
    }
}
