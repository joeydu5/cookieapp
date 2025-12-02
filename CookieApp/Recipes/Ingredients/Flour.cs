namespace CookieApp.Recipes.Ingredients;

public abstract class Flour : Ingredient
{
    public override string PreparationInstructions => $"Steve, {base.PreparationInstructions}";
}