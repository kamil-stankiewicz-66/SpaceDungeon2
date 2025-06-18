public static class SNumConverterEx
{
    public static string ToRomanNum(this int number)
    {
        if (number < 1 || number > 3999)
        {
            return "Nieobs³ugiwana liczba";
        }

        var map = new[]
        {
            new { Value = 1000, Symbol = "M" },
            new { Value = 900, Symbol = "CM" },
            new { Value = 500, Symbol = "D" },
            new { Value = 400, Symbol = "CD" },
            new { Value = 100, Symbol = "C" },
            new { Value = 90, Symbol = "XC" },
            new { Value = 50, Symbol = "L" },
            new { Value = 40, Symbol = "XL" },
            new { Value = 10, Symbol = "X" },
            new { Value = 9, Symbol = "IX" },
            new { Value = 5, Symbol = "V" },
            new { Value = 4, Symbol = "IV" },
            new { Value = 1, Symbol = "I" }
        };

        var result = "";

        foreach (var entry in map)
        {
            while (number >= entry.Value)
            {
                result += entry.Symbol;
                number -= entry.Value;
            }
        }

        return result;
    }
}
