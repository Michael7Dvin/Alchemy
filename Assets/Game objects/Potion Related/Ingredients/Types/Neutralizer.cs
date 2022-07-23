public class Neutralizer : Ingredient
{
    public override void Accept(IIngredientVisitor visitor)
    {
        visitor.Visit(this);
    }
}
