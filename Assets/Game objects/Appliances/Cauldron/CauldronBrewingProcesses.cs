using UnityEngine;

public class CauldronBrewingProcesses : MonoBehaviour
{
    [SerializeField] private Potion _potion;




    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ingredient ingredient))
        {
            AddIngredient(ingredient);
        }
    }




    private void AddIngredient(Ingredient ingredient)
    {
        _potion.AddIngredient(ingredient);
        
    }
}
