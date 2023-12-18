namespace Aoc2023.Day18;

record Coords(long Row, long Col);

record Instruction(Coords Coords, long Steps);

partial class Day18 : Solution
{
    static Coords CharToCoords(char direction) =>
        direction switch
        {
            // Normal
            'L' => new Coords(Row: 0, Col: -1),
            'R' => new Coords(Row: 0, Col: 1),
            'U' => new Coords(Row: -1, Col: 0),
            'D' => new Coords(Row: 1, Col: 0),

            // Swapped
            '2' => new Coords(Row: 0, Col: -1),
            '0' => new Coords(Row: 0, Col: 1),
            '3' => new Coords(Row: -1, Col: 0),
            '1' => new Coords(Row: 1, Col: 0),

            _ => throw new Exception($"Unknown direction {direction}")
        };

    List<Instruction> ParseInput(bool isSwapped)
    {
        var instructions = new List<Instruction>();
        foreach (var line in Input)
        {
            var lineParts = line.Split(' ', 3);

            Coords coords;
            long steps;

            if (!isSwapped)
            {
                coords = CharToCoords(lineParts[0].ToCharArray().First());
                steps = long.Parse(lineParts[1]);
            }
            else
            {
                coords = CharToCoords(lineParts[2][7]);
                steps = Convert.ToInt64(lineParts[2].Substring(2, 5), 16);
            }

            instructions.Add(new Instruction(Coords: coords, Steps: steps));
        }
        return instructions;
    }

    static List<Coords> ExecuteInstructions(List<Instruction> instructions)
    {
        var row = 0L;
        var col = 0L;

        var trench = new List<Coords>();
        foreach (var instruction in instructions)
        {
            for (var i = 0; i < instruction.Steps; i++)
            {
                row += instruction.Coords.Row;
                col += instruction.Coords.Col;
                trench.Add(new Coords(Row: row, Col: col));
            }
        }

        return trench;
    }

    // https://en.wikipedia.org/wiki/Shoelace_formula
    static long TrenchArea(List<Coords> trench)
    {
        var area = 0L;
        var j = trench.Count - 1;
        for (int i = 0; i < trench.Count; i++)
        {
            area += (trench[j].Col + trench[i].Col) * (trench[j].Row - trench[i].Row);
            j = i;
        }
        return Math.Abs(area / 2);
    }

    static long BoundaryPoints(List<Instruction> instructions) =>
        instructions.Aggregate(0L, (current, i) => current + i.Steps);

    // https://en.wikipedia.org/wiki/Pick%27s_theorem
    static long InteriorPoints(long area, long boundary) => area - boundary / 2 + 1;

    static long TrenchPoints(List<Coords> trench, List<Instruction> instructions)
    {
        var area = TrenchArea(trench);
        var boundary = BoundaryPoints(instructions);
        var interior = InteriorPoints(area, boundary);
        return (boundary + interior);
    }

    public override string Part1()
    {
        var instructions = ParseInput(false);
        var trench = ExecuteInstructions(instructions);
        return TrenchPoints(trench, instructions).ToString();
    }

    public override string Part2()
    {
        var instructions = ParseInput(true);
        var trench = ExecuteInstructions(instructions);
        return TrenchPoints(trench, instructions).ToString();
    }
}
