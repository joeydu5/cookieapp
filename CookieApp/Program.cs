using System.Globalization;

var cookieRecipesApp = new CookieRecipesApp();
cookieRecipesApp.Run();

public class CookieRecipesApp
{
    private readonly RecipesRepository _recipesRepository;
    private readonly RecipesUserInteraction _recipesUserInteraction;

    public CookieRecipesApp(
        RecipesRepository recipsRepository,
        RecipesUserInteraction recipsUserInteraction
        )
    {
        _recipesRepository = recipsRepository;
        _recipesUserInteraction = recipsUserInteraction;
    }

    public void Run()
    {
        var allReceips = _recipesRepository.Read(filepath);
        _recipesUserInteraction.PrintingExistingRecips(allReceips);
        _recipesUserInteraction.PromptToCreateRecip();

        var ingrediants = _recipesUserInteraction.ReadingIngrediantsFromUser();

        if (ingrediants.Count > 0)
        {
            var recipe = new Recipe(ingrediants);
            allReceips.Add(recipe);
            _recipesRepository.Write(filepath, allReceips);
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

public class RecipesUserInteraction
{
    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void Exit()
    {
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}

public class RecipesRepository
{
}

