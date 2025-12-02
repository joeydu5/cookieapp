namespace CookieApp.Recipes.Ingredients;

public abstract class Spice : Ingredient
{
    public override string PreparationInstructions => $"Take half a spoon, {base.PreparationInstructions}";
}