using UniRx;

public class BrewedPotionState : BasePotionState
{
    public BrewedPotionState(Potion potion, ReactiveDictionary<MagicElement, int> magicElements) : base(potion, magicElements)
    {
    }

    public override void AddIngredient(Ingredient ingredient)
    {
        return;
    }
    protected override void HandleMagicElementsChange()
    {      
        return;
    }
}
