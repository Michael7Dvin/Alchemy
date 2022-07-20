using UniRx;

public class CorrespondToSingleRecipePotionState : BasePotionState
{
    public CorrespondToSingleRecipePotionState(Potion potion, ReactiveCollection<MagicElement> magicElements) : base(potion, magicElements)
    {
    }


    protected override void HandleMagicElementsChange()
    {
        if (_potion.CorrespondingRecipes.Count == 0)
        {
            _potion.SwitchPotionState<NotCorrespondToAnyRecipePotionState>();
        }
        else if (_potion.CorrespondingRecipes.Count > 1)
        {
            _potion.SwitchPotionState<CorrespondToSeveralRecipesPotionState>();
        }
    }
}
