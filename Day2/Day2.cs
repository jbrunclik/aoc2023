namespace Aoc2023.Day2;

class Day2 : Solution
{
    enum CubeColor
    {
        Red,
        Green,
        Blue,
    }

    CubeColor StringToCubeColor(string colorString)
    {
        return colorString switch
        {
            "red" => CubeColor.Red,
            "green" => CubeColor.Green,
            "blue" => CubeColor.Blue,
            _ => throw new ApplicationException($"Invalid color {colorString}")
        };
    }

    (int, Dictionary<CubeColor, int>) MaxCubes(string line)
    {
        Dictionary<CubeColor, int> maxCubes = new Dictionary<CubeColor, int>
        {
            { CubeColor.Red, 0 },
            { CubeColor.Green, 0 },
            { CubeColor.Blue, 0 },
        };

        string[] lineSplit = line.Split(":");
        (string gameData, string cubeData) = (lineSplit[0], lineSplit[1]);

        string[] gameDataSplit = gameData.Split(" ");
        int gameId = int.Parse(gameDataSplit[1]);

        string[] cubeDataSplit = cubeData.Split(";");
        foreach (string cubeBag in cubeDataSplit)
        {
            string[] cubeBagSplit = cubeBag.Split(",");
            foreach (string cubeCount in cubeBagSplit)
            {
                string[] cubeCountSplit = cubeCount.Trim().Split(" ");
                CubeColor color = StringToCubeColor(cubeCountSplit[1]);
                int count = int.Parse(cubeCountSplit[0]);
                maxCubes[color] = Math.Max(count, maxCubes[color]);
            }
        }

        return (gameId, maxCubes);
    }

    public override string Part1()
    {
        List<int> validGames = new List<int> { };

        foreach (string line in Input)
        {
            (int gameId, Dictionary<CubeColor, int> maxCubes) = MaxCubes(line);
            if (
                maxCubes[CubeColor.Red] <= 12
                && maxCubes[CubeColor.Green] <= 13
                && maxCubes[CubeColor.Blue] <= 14
            )
            {
                validGames.Add(gameId);
            }
        }

        return validGames.Sum().ToString();
    }

    public override string Part2()
    {
        List<int> powers = new List<int> { };

        foreach (string line in Input)
        {
            (_, Dictionary<CubeColor, int> maxCubes) = MaxCubes(line);
            int power =
                maxCubes[CubeColor.Red] * maxCubes[CubeColor.Green] * maxCubes[CubeColor.Blue];
            powers.Add(power);
        }

        return powers.Sum().ToString();
    }
}
