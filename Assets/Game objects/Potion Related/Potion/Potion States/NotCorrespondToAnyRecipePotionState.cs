using UniRx;

public class NotCorrespondToAnyRecipePotionState : BasePotionState
{
    public NotCorrespondToAnyRecipePotionState(Potion potion, ReactiveCollection<MagicElement> magicElements) : base(potion, magicElements)
    {
    }


    protected override void HandleMagicElementsChange()
    {
        if(_potion.CorrespondingRecipes.Count == 1)
        {
            _potion.SwitchPotionState<CorrespondToSingleRecipePotionState>();
        }
        else if(_potion.CorrespondingRecipes.Count > 1)
        {
            _potion.SwitchPotionState<CorrespondToSeveralRecipesPotionState>();
        }
    }
}
