using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private List<MagicElement> _magicElements;
    [SerializeField] private float _amplifyFactor;


    public IReadOnlyList<MagicElement> MagicElements => _magicElements;
    public float AmplifyFactor => _amplifyFactor;


    public void AddToPotion()
    {
        Destroy(gameObject);
    }    
}