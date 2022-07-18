using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;

public class Potion : Liquid
{
    private ReactiveDictionary<MagicElement, int> _observableMagicElements = new ReactiveDictionary<MagicElement, int>();

    private BasePotionState _currentPotionState;
    private List<BasePotionState> _potionStates;

    private PotionRecipesDataBase _potionRecipesDataBase;


    public IReadOnlyReactiveDictionary<MagicElement, int> ObservableMagicElements => _observableMagicElements;

    public List<PotionRecipe> CorrespondingRecipes => _potionRecipesDataBase.GetCorrespondingRecipes(_observableMagicElements);

    private List<BasePotionState> PotionStates => _potionStates;
    
    private Dictionary<MagicElement, int> ExccessMagicElements
    {
        get
        {
            if(CorrespondingRecipes.Count == 0 | CorrespondingRecipes.Count > 1)
            {
                Debug.LogError("Unavailable to get ExccessMagicElements");
                return null;
            }


            Dictionary<MagicElement, int> exccessElements = new Dictionary<MagicElement, int>();

            foreach (var element in _observableMagicElements)
            {
                if (CorrespondingRecipes[0].Elements.ContainsKey(element.Key) == true)
                {
                    int exccess = element.Value - (CorrespondingRecipes[0].Elements[element.Key] * PotionAmount);

                    if (exccess > 0)
                    {
                        exccessElements.Add(element.Key, exccess);
                    }
                }
                else
                {
                    exccessElements.Add(element.Key, element.Value);
                }
            }

            return exccessElements;
        }
    }

    private int PotionAmount
    {
        get 
        {
            if (CorrespondingRecipes.Count == 0 | CorrespondingRecipes.Count > 1)
            {
                Debug.LogError("Unavailable to get PotionAmount");
                return 0;
            }


            List<int> potionAmounts = new List<int>();

            foreach (var element in CorrespondingRecipes[0].Elements)
            {
                float quotient = _observableMagicElements[element.Key] / element.Value;

                int amount = Convert.ToInt32(Mathf.Floor(quotient));

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
