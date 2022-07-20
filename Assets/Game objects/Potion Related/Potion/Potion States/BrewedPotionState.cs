using UniRx;

public class BrewedPotionState : BasePotionState
{
    public BrewedPotionState(Potion potion, ReactiveCollection<MagicElement> magicElements) : base(potion, magicElements)
    {
    }


    public override void AddIngredient(Ingredient ingredient) { }

    protected override void HandleMagicElementsChange() { }
}
