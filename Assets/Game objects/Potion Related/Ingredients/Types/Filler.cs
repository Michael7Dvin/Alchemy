using System.Collections.Generic;
using UnityEngine;

public class Filler : Ingredient
{
    [SerializeField] private List<MagicElement> _magicElements;

    public IReadOnlyList<MagicElement> MagicElements => _magicElements;

    public override void Accept(IIngredientVisitor visitor)
    {
        visitor.Visit(this);
    }
}
