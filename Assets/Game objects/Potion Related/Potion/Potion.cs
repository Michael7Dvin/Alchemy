using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;

public class Potion : Liquid
{
    //private ReactiveCollection<MagicElement> _observableMagicElements = new ReactiveCollection<MagicElement>();

    private readonly PotionMagicElements _magicElements = new PotionMagicElements();

    private BasePotionState _currentPotionState;
    private List<BasePotionState> _potionStates;

    private PotionRecipesDataBase _potionRecipesDataBase;


    public IReadOnlyReactiveCollection<MagicElement> ObservableMagicElements => _magicElements.ObservableMagicElements;

    public List<PotionRecipe> CorrespondingRecipes => _potionRecipesDataBase.GetCorrespondingRecipes(ObservableMagicElements.ToList());

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

            foreach(MagicElement magicElement in ObservableMagicElements.Distinct())
            {
                if(CorrespondingRecipes[0].MagicElements.Contains(magicElement))
                {
                    int MagicElementAmountAtPotion = ObservableMagicElements.Count(x => x == magicElement);
                    int MagicElementAmountAtRecipe = CorrespondingRecipes[0].MagicElements.Count(x => x == magicElement) * PotionAmount;

                    int amountExccess = MagicElementAmountAtPotion - MagicElementAmountAtRecipe;

                    AddMagicElements(magicElement, amountExccess);
                }
                else
                {
                    int amount = ObservableMagicElements.Count(x => x == magicElement);
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
                int MagicElementAmountAtPotion = ObservableMagicElements.Count(x => x == magicElement);
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
            new NotCorrespondToAnyRecipePotionState(this, _magicElements),
            new CorrespondToSingleRecipePotionState(this, _magicElements),
            new CorrespondToSeveralRecipesPotionState(this, _magicElements),
            new BrewingPotionState(this, _magicElements),
            new BrewedPotionState(this, _magicElements)
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
