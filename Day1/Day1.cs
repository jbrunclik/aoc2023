using System.Text.RegularExpressions;

namespace Aoc2023.Day1;

class Day1 : Solution
{
    public override string Part1()
    {
        int sum = 0;
        Regex digitRegex = new Regex(@"\d");

        foreach (string line in Input)
        {
            MatchCollection matches = digitRegex.Matches(line);
            string first = matches.First().Value;
            string last = matches.Last().Value;
            sum += int.Parse($"{first}{last}");
        }

        return sum.ToString();
    }

    private int DigitToInt(string digit)
    {
        if (int.TryParse(digit, out var d))
        {
            return d;
        }

        return digit switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => throw new ApplicationException($"Unable to convert {digit} to int")
        };
    }

    public override string Part2()
    {
        int sum = 0;

        foreach (string line in Input)
        {
            string digits = @"\d|one|two|three|four|five|six|seven|eight|nine";
            Regex leftRegex = new Regex(digits);
            Regex rightRegex = new Regex(digits, RegexOptions.RightToLeft);

            int first = DigitToInt(leftRegex.Match(line).Value);
            int last = DigitToInt(rightRegex.Match(line).Value);
            int code = int.Parse($"{first}{last}");
            sum += code;
        }

        return sum.ToString();
    }
}
