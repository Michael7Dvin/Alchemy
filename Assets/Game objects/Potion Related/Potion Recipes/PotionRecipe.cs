using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Potion Recipe")]
public class PotionRecipe : MagicElementDictionary
{
    [SerializeField] private string _name;


    public string Name => _name;
}
