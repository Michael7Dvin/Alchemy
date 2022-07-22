using UniRx;

public abstract class BasePotionState
{
    protected PotionMagicElements _potionMagicElements;
    
    protected readonly Potion _potion;


    protected BasePotionState(Potion potion, PotionMagicElements potionMagicElements)
    {
        _potion = potion;
        _potionMagicElements = potionMagicElements;

        _potion.ObservableMagicElements.ObserveAdd().Subscribe(_ => HandleMagicElementsChange());
        _potion.ObservableMagicElements.ObserveReplace().Subscribe(_ => HandleMagicElementsChange());
        _potion.ObservableMagicElements.ObserveRemove().Subscribe(_ => HandleMagicElementsChange());
    }


    public virtual void Enter() { }               
    public virtual void Exit() { }

    public virtual void AddIngredient(Ingredient ingredient)
    {
        if (_potion.IsCurrentLiquidStateEqualsT<BoilingLiquidState>())
        {
            foreach( MagicElement ingredientMagicElement in ingredient.MagicElements)
            {
                _potionMagicElements.AddMagicElement(ingredientMagicElement);
            }
        }
    }
    
    protected abstract void HandleMagicElementsChange();
}