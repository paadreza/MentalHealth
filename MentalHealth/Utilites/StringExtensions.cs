namespace MentalHealth.Utils
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var words = System.Text.RegularExpressions.Regex.Matches(input, @"([A-Z][a-z]+)|([0-9]+)")
                .Cast<System.Text.RegularExpressions.Match>()
                .Select(m => m.Value);

            return string.Join(" ", words);
        }
    }
}