public class Amplifier : Ingredient
{
    public override void Accept(IIngredientVisitor visitor)
    {
        visitor.Visit(this);
    }
}
