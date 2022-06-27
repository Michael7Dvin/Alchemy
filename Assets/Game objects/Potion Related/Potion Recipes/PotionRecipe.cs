using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Potion Recipe")]
public class PotionRecipe : ScriptableObject
{
    [SerializeField] private List<IngredientMagicElement> _keys;
    [Range(1f, 20f)]
    [SerializeField] private List<int> _values;

    public Dictionary<IngredientMagicElement, int> _elements;
    public Dictionary<IngredientMagicElement, int> Elements => _elements;

    private void OnValidate()
    {
        _elements = TrySerializeDictionary();
    }

    public bool IsVaild
    {
        get
        {
            if(Elements == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
   

    private Dictionary<IngredientMagicElement, int> TrySerializeDictionary()
    {
        if(_keys.Count <= 0 || _values.Count <= 0)
        {
            return null;
        }

        if(_keys.Count != _values.Count)
        {
            return null;
        }

        int dictionaryLength = _keys.Count;
        
        Dictionary<IngredientMagicElement, int> potionRecipeElements = new Dictionary<IngredientMagicElement, int>();

        for (int i = 0; i < dictionaryLength; i++)
        {
            if(_keys[i] != null && _values[i] > 0)
            {
                potionRecipeElements.Add(_keys[i], _values[i]);
            }
            else
            {
                return null;
            }    
        }

        return potionRecipeElements;
    }
}
