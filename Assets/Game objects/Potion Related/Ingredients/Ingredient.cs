using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private IngredientMagicElements _ingredientMagicElements;
    [SerializeField] private float _amplifyFactor;


    public string Name => _name;
    public string Description => _description;
    public IngredientMagicElements MagicElements => _ingredientMagicElements;
    public float AmplifyFactor => _amplifyFactor;


    public void AddToPotion()
    {
        Destroy(gameObject);
    }    
}