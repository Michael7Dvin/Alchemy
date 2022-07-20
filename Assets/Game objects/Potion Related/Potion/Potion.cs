using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;

public class Potion : Liquid
{
    private ReactiveCollection<MagicElement> _observableMagicElements = new ReactiveCollection<MagicElement>();

    private BasePotionState _currentPotionState;
    private List<BasePotionState> _potionStates;

    private PotionRecipesDataBase _potionRecipesDataBase;


    public IReadOnlyReactiveCollection<MagicElement> ObservableMagicElements => _observableMagicElements;

    public List<PotionRecipe> CorrespondingRecipes => _potionRecipesDataBase.GetCorrespondingRecipes(_observableMagicElements.ToList());

    private List<BasePotionState> PotionStates => _potionStates;
    
    private List<MagicElement> ExccessMagicElements
    {
        get
        {
            if(CorrespondingRecipes.Count == 0 | CorrespondingRecipes.Count > 1)
            {
                return new List<MagicElement>();
            }

            List<MagicElement> exccessMagicElements = new List<MagicElement>();

            foreach(MagicElement magicElement in _observableMagicElements.Distinct())
            {
                if(CorrespondingRecipes[0].MagicElements.Contains(magicElement))
                {
                    int MagicElementAmountAtPotion = _observableMagicElements.Count(x => x == magicElement);
                    int MagicElementAmountAtRecipe = CorrespondingRecipes[0].MagicElements.Count(x => x == magicElement) * PotionAmount;

                    int amountExccess = MagicElementAmountAtPotion - MagicElementAmountAtRecipe;

                    AddMagicElements(magicElement, amountExccess);
                }
                else
                {
                    int amount = _observableMagicElements.Count(x => x == magicElement);
                    AddMagicElements(magicElement, amount);
                }
            }

            void AddMagicElements(MagicElement magicElement, int amount)
            {
                for(int i = 0; i < amount; i++)
                {
                    exccessMagicElements.Add(magicElement);
                }
            }
           
            return exccessMagicElements;
        }
    }

    private int PotionAmount
    {
        get 
        {
            if (CorrespondingRecipes.Count == 0 | CorrespondingRecipes.Count > 1)
            {
                return 0;
            }

            List<int> potionAmounts = new List<int>();

            foreach (MagicElement magicElement in CorrespondingRecipes[0].MagicElements)
            {
                int MagicElementAmountAtPotion = _observableMagicElements.Count(x => x == magicElement);
                int MagicElementAmountAtRecipe = CorrespondingRecipes[0].MagicElements.Count(x => x == magicElement);

                float magicElementAmountQuotient = MagicElementAmountAtPotion / MagicElementAmountAtRecipe;

                int amount = Convert.ToInt32(Mathf.Floor(magicElementAmountQuotient));

                potionAmounts.Add(amount);
            }

            return potionAmounts.Min();      
        }
    }
        

    [Inject]
    private void Construct(PotionRecipesDataBase potionRecipesDataBase)
    {
        _potionRecipesDataBase = potionRecipesDataBase;
    }


    private new void Start()
    {
        base.Start();

        _potionStates = new List<BasePotionState>()
        {
            new NotCorrespondToAnyRecipePotionState(this, _observableMagicElements),
            new CorrespondToSingleRecipePotionState(this, _observableMagicElements),
            new CorrespondToSeveralRecipesPotionState(this, _observableMagicElements),
            new BrewingPotionState(this, _observableMagicElements),
            new BrewedPotionState(this, _observableMagicElements)
        };
        SwitchPotionState<NotCorrespondToAnyRecipePotionState>();
    }


    public void AddIngredient(Ingredient ingredient)
    {
        _currentPotionState.AddIngredient(ingredient);
    }

    public void SwitchPotionState<T>() where T : BasePotionState
    {
        BasePotionState state = _potionStates.FirstOrDefault(s => s is T);

        if (state != _currentPotionState)
        { 
            _currentPotionState?.Exit();
            state.Enter();
            _currentPotionState = state;        
        }
    }
}
