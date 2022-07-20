using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Potion Recipe")]
public class PotionRecipe : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private List<MagicElement> _magicElements;

    public string Name => _name;
    public IReadOnlyList<MagicElement> MagicElements => _magicElements;
}
