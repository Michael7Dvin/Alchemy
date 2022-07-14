using System.Collections.Generic;
using UnityEngine;

public class MagicElementDictionary : ScriptableObject
{
    [SerializeField] private List<MagicElement> _keys;
    [SerializeField] private List<int> _values;

    public Dictionary<MagicElement, int> _elements;
    public Dictionary<MagicElement, int> Elements => _elements;


    public bool IsVaild
    {
        get
        {
            if (Elements == null)
            {                
                return false;
            }
            else
            {
                return true;
            }
        }
    }


    private void OnValidate()
    {
        _elements = TrySerializeDictionary();
    }


    private Dictionary<MagicElement, int> TrySerializeDictionary()
    {
        if (_keys.Count <= 0 || _values.Count <= 0)
        {
            Debug.LogWarning($"[{this}]: Dictionary must contain at least 1 element");
            return null;
        }

        if (_keys.Count != _values.Count)
        {
            Debug.LogWarning($"[{this}]: Number of keys and values must be the same");
            return null;
        }

        int dictionaryLength = _keys.Count;

        Dictionary<MagicElement, int> potionRecipeElements = new Dictionary<MagicElement, int>();

        for (int i = 0; i < dictionaryLength; i++)
        {
            if (_keys[i] != null && _values[i] > 0)
            {
                potionRecipeElements.Add(_keys[i], _values[i]);
            }
            else if(_keys[i] == null)
            {
                Debug.LogWarning($"[{this}]: Key cannot be null");
                return null;
            }
            else if(_values[i] <= 0)
            {
                Debug.LogWarning($"[{this}]: Value must be greater than zero");
                return null;
            }
        }

        return potionRecipeElements;
    }
}
