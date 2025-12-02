using CookieApp.Recipes;
using CookieApp.Recipes.Ingredients;

var cookieRecipesApp = new CookieRecipesApp(new RecipesRepository(new StringTextualRepository(), new IngredientsRegister()), new RecipesConsoleUserInteraction(new IngredientsRegister()));
cookieRecipesApp.Run("recipes.txt");

public class CookieRecipesApp
{
    private readonly IRecipesRepository _recipesRepository;
    private readonly IRecipesUserInteraction _recipesUserInteraction;

    public CookieRecipesApp(
        IRecipesRepository recipesRepository,
        IRecipesUserInteraction recipesUserInteraction
        )
    {
        _recipesRepository = recipesRepository;
        _recipesUserInteraction = recipesUserInteraction;
    }

    public void Run(string filepath)
    {
        var allRecipes = _recipesRepository.Read(filepath);
        _recipesUserInteraction.PrintingExistingRecips(allRecipes);
        _recipesUserInteraction.PromptToCreateRecipe();
        
        var ingredients = _recipesUserInteraction.ReadIngredientsFromUser();
        
        if (ingredients.Count() > 0)
        {
            var recipe = new Recipe(ingredients);
            allRecipes.Add(recipe);
            _recipesRepository.Write(filepath, allRecipes);
            _recipesUserInteraction.ShowMessage("Recipe Added!");
            _recipesUserInteraction.ShowMessage(recipe.ToString());
        }
        else
        {
            _recipesUserInteraction.ShowMessage("No ingrediants has been selected, recipe will not be saved.");
        }

        _recipesUserInteraction.Exit();
    }
    
    

}

public interface IRecipesUserInteraction
{
    void ShowMessage(string message);
    void Exit();
    void PrintingExistingRecips(IEnumerable<Recipe> allRecipes);
    void PromptToCreateRecipe();
    IEnumerable<Ingredient> ReadIngredientsFromUser();

}

public interface IRecipesRepository
{
    List<Recipe> Read(string filepath);

    void Write(string filepath, List<Recipe> allRecipes);
}

public class RecipesConsoleUserInteraction:IRecipesUserInteraction
{

    private readonly IIngredientsRegister _ingredientsRegister;

    public RecipesConsoleUserInteraction(IIngredientsRegister ingredientsRegister)
    {
        _ingredientsRegister = ingredientsRegister;
    }
    
    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void Exit()
    {
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    public void PrintingExistingRecips(IEnumerable<Recipe> allRecipes)
    {
        if (allRecipes.Count() > 0)
        {
            Console.WriteLine("Existing recipes are: " + Environment.NewLine);
            var count = 1;
            foreach (var recipe in allRecipes)
            {
                Console.WriteLine($"*** {count} *** - {recipe}");
                Console.WriteLine();
                count++;
            }
        }
    }

    public void PromptToCreateRecipe()
    {
        Console.WriteLine("Pick some ingredients from: ");
        foreach (var ingredient in _ingredientsRegister.All)
        {
            Console.WriteLine(ingredient);
        }
    }

    public IEnumerable<Ingredient> ReadIngredientsFromUser()
    {
        var ingredients = new List<Ingredient>();
        var shallStop = false;
        while (!shallStop)
        {
            Console.WriteLine("Pick any ingredient by selecting the ID: ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int id))
            {
                var selectedIngredient = _ingredientsRegister.GetById(id);
                if (selectedIngredient is not null)
                {
                    ingredients.Add(selectedIngredient);
                }
            }
            else
            {
                shallStop = true;
            }
        }
        return ingredients;
    }
}

public class RecipesRepository : IRecipesRepository
{
    private readonly IStringRepository _stringRepository;
    private readonly IIngredientsRegister _ingredientsRegister;

    public RecipesRepository(IStringRepository stringRepository, IIngredientsRegister ingredientsRegister)
    {
        _stringRepository = stringRepository;
        _ingredientsRegister = ingredientsRegister;
    }
    public List<Recipe> Read(string filepath)
    {
       var allRecipesString = _stringRepository.Read(filepath);
       var recipes = new List<Recipe>();
       foreach (var recipeString in allRecipesString)
       {
           var recipe = RecipeFromFile(recipeString);
           recipes.Add(recipe);
       }
       return recipes;
    }

    private Recipe RecipeFromFile(string recipeString)
    {
          
        var textualIds =  recipeString.Split(",");
        var ingredients = new List<Ingredient>();
        foreach (var ingredientString in textualIds)
        {
            var ingredient = _ingredientsRegister.GetById(int.Parse(ingredientString));
            ingredients.Add(ingredient);
        }

        return new Recipe(ingredients);
    }

    public void Write(string filepath, List<Recipe> allRecipes)
    {
        var recipesAsString = new List<string>();
        foreach (var recipe in allRecipes)
        {
            var allIds = new List<int>();
            foreach (var ingredient in recipe.Ingredients)
            {
                allIds.Add(ingredient.Id);
            }
            recipesAsString.Add(string.Join(",", allIds));
        }
        
        _stringRepository.Write(filepath, recipesAsString);
    }
}

public interface IStringRepository
{
    List<string> Read(string filepath);
    void Write(string filepath, List<string> strings);
}

public class StringTextualRepository : IStringRepository
{
    private static readonly string Seperator = Environment.NewLine;

    public List<string> Read(string filepath)
    {
        if (File.Exists(filepath))
        {
            var fileContent = File.ReadAllText(filepath);
            return fileContent.Split(Seperator).ToList();
        }
        return new List<string>();
    }

    public void Write(string filepath, List<string> strings)
    {
        File.WriteAllText(filepath, string.Join(Seperator, strings));
    }
}

