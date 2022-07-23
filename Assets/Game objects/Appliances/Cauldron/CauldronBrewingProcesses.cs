using System.Collections.Generic;
using UnityEngine;

public class CauldronBrewingProcesses : MonoBehaviour
{
    [SerializeField] private Potion _potion;
    

    private void OnTriggerEnter(Collider other)
    {        
        if (other.TryGetComponent(out Ingredient result))
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            other.GetComponents(ingredients);

            foreach (Ingredient ingredient in ingredients)
            {
                AddIngredient(ingredient);
            }
        }
    }


    private void AddIngredient(Ingredient ingredient)
    {
        _potion.AddIngredient(ingredient);
        
    }
}
