using CookieApp.Recipes.Ingredients;

namespace CookieApp.Recipes;

public class Recipe
{
    public IEnumerable<Ingredient> Ingredients { get; }
    
    public Recipe(IEnumerable<Ingredient> ingredients )
    {
        Ingredients = ingredients; 
    }

    public override string ToString()
    {
        var result = new List<string>();
        var count = 1;
        foreach (var each in Ingredients)
        {
            result.Add($"{Environment.NewLine}{count}-{each.Name} - {each.PreparationInstructions}");
            count++;
        }
        return string.Join(", ", result);
    }
}