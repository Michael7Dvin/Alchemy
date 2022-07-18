using System.Collections.Generic;
using UniRx;

public abstract class BasePotionState
{
    protected ReactiveDictionary<MagicElement, int> _magicElements;
    
    protected readonly Potion _potion;


    protected BasePotionState(Potion potion, ReactiveDictionary<MagicElement, int> magicElements)
    {
        _potion = potion;
        _magicElements = magicElements;

        _potion.ObservableMagicElements.ObserveAdd().Subscribe(_ => HandleMagicElementsChange());
        _potion.ObservableMagicElements.ObserveReplace().Subscribe(_ => HandleMagicElementsChange());
        _potion.ObservableMagicElements.ObserveRemove().Subscribe(_ => HandleMagicElementsChange());
    }


    public virtual void Enter()
    {        
    }

    public virtual void Exit()
    {
    }
    
    public virtual void AddIngredient(Ingredient ingredient)
    {      
        foreach (KeyValuePair<MagicElement, int> ingredientMagicElement in ingredient.MagicElements.Elements)
        {
            if (_magicElements.ContainsKey(ingredientMagicElement.Key))
            {
                _magicElements[ingredientMagicElement.Key] += ingredientMagicElement.Value;
            }
            else
            {
                _magicElements.Add(ingredientMagicElement.Key, ingredientMagicElement.Value);
            }
        }
    }
    
    protected abstract void HandleMagicElementsChange();
}