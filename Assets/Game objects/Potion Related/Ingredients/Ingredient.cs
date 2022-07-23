using UnityEngine;

public abstract class Ingredient : MonoBehaviour
{
    public abstract void Accept(IIngredientVisitor visitor);
   
    public virtual void AddToPotion()
    {
        Destroy(gameObject);
    }    
}