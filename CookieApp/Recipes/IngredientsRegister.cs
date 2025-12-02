using CookieApp.Recipes.Ingredients;

namespace CookieApp.Recipes;

public interface IIngredientsRegister
{
    IEnumerable<Ingredient> All { get; }
    Ingredient GetById(int id);
}

public class IngredientsRegister : IIngredientsRegister
{
    public IEnumerable<Ingredient> All { get; } = new List<Ingredient>()
    {
        new WheatFlour(),
        new SpeltFlour(),
        new Butter(),
        new Chocolate(),
        new Sugar(),
        new Cardamom(),
        new Cinnamon(),
    };

    public Ingredient GetById(int id)
    {
        foreach (var eachIngredient in All)
        {
            if (eachIngredient.Id == id)
            {
                return eachIngredient;
            }

        }
        return null;
    }
}

