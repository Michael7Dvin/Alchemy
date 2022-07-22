using System.Collections.Generic;
using System.Linq;
using UniRx;

public class PotionMagicElements
{
    private ReactiveCollection<MagicElement> _observableMagicElements = new ReactiveCollection<MagicElement>();


    public IReadOnlyReactiveCollection<MagicElement> ObservableMagicElements => _observableMagicElements;


    public void AddMagicElement(MagicElement magicElement)
    {
        if (GetOppositeMagicElements(magicElement).Count == 0)
        {
            _observableMagicElements.Add(magicElement);
        }
        else
        {
            TryAddOppositeElement(magicElement);
        }
    }

    private List<MagicElement> GetOppositeMagicElements(MagicElement magicElement)
    {
        if (magicElement.OppositeMagicElementTypes.Count == 0)
        {
            return new List<MagicElement>();
        }

        return _observableMagicElements
            .Where(x => magicElement.OppositeMagicElementTypes.Contains(x.Type))
            .ToList();
    }

    private void TryAddOppositeElement(MagicElement magicElement)
    {
        System.Random random = new System.Random();

        _observableMagicElements = _observableMagicElements
            .OrderBy(x => random.Next())
            .ToReactiveCollection();

        foreach (MagicElement oppositeMagicElement in GetOppositeMagicElements(magicElement))
        {
            _observableMagicElements.Remove(oppositeMagicElement);
            MagicElement confrontedMagicElement = ConfrontOppositeElements(magicElement, oppositeMagicElement);

            if (confrontedMagicElement == null)
            {
                magicElement = null;
                break;
            }
            else if (confrontedMagicElement.Type == oppositeMagicElement.Type)
            {
                magicElement = null;
                _observableMagicElements.Add(confrontedMagicElement);
                break;
            }
            else if (confrontedMagicElement.Type == magicElement.Type)
            {
                magicElement = confrontedMagicElement;
            }
        }

        if (magicElement != null)
        {
            _observableMagicElements.Add(magicElement);
        }
    }

    private MagicElement ConfrontOppositeElements(MagicElement magicElement, MagicElement oppositeMagicElement)
    {
        if (magicElement.Strength > oppositeMagicElement.Strength)
        {
            if (magicElement.Strength - oppositeMagicElement.Strength == 1)
            {
                return magicElement.LessBy1StrengthMagicElement;
            }
            return magicElement;

        }
        else if (magicElement.Strength < oppositeMagicElement.Strength)
        {
            if (oppositeMagicElement.Strength - magicElement.Strength == 1)
            {
                return oppositeMagicElement.LessBy1StrengthMagicElement;

            }
            return oppositeMagicElement;
        }

        return null;
    }
}
