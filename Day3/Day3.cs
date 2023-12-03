namespace Aoc2023.Day3;

class Day3 : Solution
{
    private char[,] InputToCharArray()
    {
        int y = Input.Length;
        int x = Input[0].Length;
        var a = new char[y, x];
        for (int i = 0; i < Input.Length; i++)
        {
            for (int j = 0; j < Input[i].Length; j++)
            {
                a[i, j] = Input[i].ElementAt(j);
            }
        }

        return a;
    }

    private record PartNumber(int Number, int StartX, int EndX, int Y);

    private List<PartNumber> GetPartNumbers(char[,] schema)
    {
        var partNumbers = new List<PartNumber> { };

        for (int y = 0; y < schema.GetLength(1); y++)
        {
            for (int x = 0; x < schema.GetLength(0); x++)
            {
                var number = new List<char> { };
                int startX = x;
                int endX = x;

                while (Char.IsDigit(schema[y, x]))
                {
                    number.Add(schema[y, x]);
                    endX = x;

                    if (x == schema.GetLength(0) - 1)
                    {
                        break;
                    }

                    x++;
                }

                if (number.Count > 0)
                {
                    var parsedNumber = int.Parse(string.Join("", number));
                    var partNumber = new PartNumber(parsedNumber, startX, endX, y);
                    partNumbers.Add(partNumber);
                }
            }
        }

        return partNumbers;
    }

    private record GearCoords(int X, int Y);

    private (bool, GearCoords?) IsValidPartNumber(char[,] schema, PartNumber partNumber)
    {
        var isValid = false;
        GearCoords? gearCoords = null;

        for (int y = partNumber.Y - 1; y <= partNumber.Y + 1; y++)
        {
            for (int x = partNumber.StartX - 1; x <= partNumber.EndX + 1; x++)
            {
                if ((x < 0) || (x >= schema.GetLength(0)) || (y < 0) || (y >= schema.GetLength(1)))
                {
                    continue;
                }

                if (char.IsDigit(schema[y, x]) || schema[y, x] == '.')
                {
                    continue;
                }

                isValid = true;
                if (schema[y, x] == '*')
                {
                    gearCoords = new GearCoords(x, y);
                }
            }
        }

        return (isValid, gearCoords);
    }

    public override string Part1()
    {
        var schema = InputToCharArray();
        var partNumbers = GetPartNumbers(schema);

        var sum = 0;
        foreach (var partNumber in partNumbers)
        {
            (bool isValid, _) = IsValidPartNumber(schema, partNumber);
            if (isValid)
            {
                sum += partNumber.Number;
            }
        }

        return sum.ToString();
    }

    public override string Part2()
    {
        var schema = InputToCharArray();
        var partNumbers = GetPartNumbers(schema);

        var gears = new Dictionary<GearCoords, List<PartNumber>> { };
        foreach (var partNumber in partNumbers)
        {
            (_, GearCoords? gearCoords) = IsValidPartNumber(schema, partNumber);
            if (gearCoords != null)
            {
                if (!gears.ContainsKey(gearCoords))
                {
                    gears[gearCoords] =  [];
                }
                gears[gearCoords].Add(partNumber);
            }
        }

        var sum = 0;
        foreach (var gear in gears.Values)
        {
            if (gear.Count == 1)
            {
                continue;
            }
            sum += gear.ElementAt(0).Number * gear.ElementAt(1).Number;
        }

        return sum.ToString();
    }
}
