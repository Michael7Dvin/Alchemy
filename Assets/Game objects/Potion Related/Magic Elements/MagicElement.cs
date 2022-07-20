using System.Collections.Generic;
using UnityEngine;

public enum MagicElementTypes
{
    None = 0,
    Life = 1,
    Earth = 2,
    Blood = 3,
    Electricity = 4,
    Defence = 5,
    Speed = 6,
    Poison = 7,
    Death = 8,
    Lightness = 9,
    Light = 10
}

[CreateAssetMenu(menuName = "Scriptable Objects/Magic Element")]
public class MagicElement : ScriptableObject
{
    [SerializeField] private MagicElementTypes _type;
    [SerializeField] private int _strength;

    [SerializeField] private List<MagicElementTypes> _oppositeMagicElementTypes;

    [SerializeField] private MagicElement _lessBy1StrengthMagicElement;
    [SerializeField] private MagicElement _moreBy1StrengthMagicElement;


    public MagicElementTypes Type => _type;
    public int Strength => _strength;

    public List<MagicElementTypes> OppositeMagicElementTypes => _oppositeMagicElementTypes;

    public MagicElement LessBy1StrengthMagicElement => _lessBy1StrengthMagicElement;
    public MagicElement MoreBy1StrengthMagicElement => _moreBy1StrengthMagicElement;
}
