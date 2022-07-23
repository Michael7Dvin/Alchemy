public interface IIngredientVisitor 
{
    public void Visit(Filler ingredient);
    public void Visit(Amplifier ingredient);
    public void Visit(Neutralizer ingredient);
}
