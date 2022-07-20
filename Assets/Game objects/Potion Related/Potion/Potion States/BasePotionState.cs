using System.Collections.Generic;
using UniRx;

public abstract class BasePotionState
{
    protected ReactiveCollection<MagicElement> _potionMagicElements;
    
    protected readonly Potion _potion;


    protected BasePotionState(Potion potion, ReactiveCollection<MagicElement> magicElements)
    {
        _potion = potion;
        _potionMagicElements = magicElements;

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
                _potionMagicElements.Add(ingredientMagicElement);
            }
        }
    }
    
    protected abstract void HandleMagicElementsChange();
}