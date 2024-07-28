using StajYerApp_API;
public static class Utilities
{
    public static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        input = input.ToLower();
        return char.ToUpper(input[0]) + input.Substring(1);
    }
}
