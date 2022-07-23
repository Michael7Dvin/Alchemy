using UniRx;

public class BrewedPotionState : BasePotionState
{
    public BrewedPotionState(Potion potion, PotionMagicElements potionMagicElements) : base(potion, potionMagicElements)
    {
    }


    public override void AddIngredient(Ingredient ingredient) { }

    protected override void HandleMagicElementsChange() { }
}
