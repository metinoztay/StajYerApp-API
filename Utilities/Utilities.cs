using StajYerApp_API;
using System.Text;
public static class Utilities
{
    public static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        input = input.ToLower();
        return char.ToUpper(input[0]) + input.Substring(1);
    }

    public static string GenerateRandomPassword(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder password = new StringBuilder();
        Random random = new Random();

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(chars.Length);
            password.Append(chars[index]);
        }

        return password.ToString();
    }

}
