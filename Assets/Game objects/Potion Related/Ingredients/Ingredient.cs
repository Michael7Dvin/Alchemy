using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private List<IngredientMagicElement> _magicElements = new();
    [SerializeField] private float _amplifyFactor;


    public string Name => _name;
    public string Description => _description; 
    public List<IngredientMagicElement> MagicElements => _magicElements;
    public float AmplifyFactor => _amplifyFactor;


    public void AddToPotion()
    {
        Destroy(gameObject);
    }    
}