using UnityEngine;
using UniRx;

public abstract class BasePotionState : IIngredientVisitor
{
    protected readonly Potion _potion;
    protected PotionMagicElements _potionMagicElements;


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
        ingredient.Accept(this);        
    }



    protected abstract void HandleMagicElementsChange();



    public virtual void Visit(Filler ingredient)
    {
        if (_potion.IsCurrentLiquidStateEqualsT<BoilingLiquidState>())
        {
            foreach (MagicElement ingredientMagicElement in ingredient.MagicElements)
            {
                _potionMagicElements.AddMagicElement(ingredientMagicElement);
            }
        }
    }

    public virtual void Visit(Amplifier ingredient)
    {
        Debug.Log("amplified");
    }

    public virtual void Visit(Neutralizer ingredient)
    {
        Debug.Log("Nutralized");
    }
}