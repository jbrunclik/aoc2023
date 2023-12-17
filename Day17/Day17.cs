namespace Aoc2023.Day17;

record Coords(int Row, int Col);

record Block(Coords Coords, Coords? Direction, int Consecutive);

record City(Dictionary<Coords, int> Blocks, int LastRow, int LastCol);

partial class Day17 : Solution
{
    City ParseInput()
    {
        var blocks = Input
            .SelectMany(
                (line, row) =>
                    line.ToCharArray()
                        .Select(
                            (weight, col) =>
                                (new Coords(Row: row, Col: col), (int)Char.GetNumericValue(weight))
                        )
            )
            .ToDictionary(w => w.Item1, w => w.Item2);
        return new City(Blocks: blocks, LastRow: Input.Length - 1, LastCol: Input[0].Length - 1);
    }

    int FindLowestHeatLoss(City city)
    {
        var visited = new HashSet<Block>();
        var queue = new PriorityQueue<Block, int>();
        queue.Enqueue(
            new Block(Coords: new Coords(Row: 0, Col: 0), Direction: null, Consecutive: 0),
            0
        );

        var lastBlockCoords = new Coords(Row: city.LastRow, Col: city.LastCol);
        while (queue.TryDequeue(out var block, out var heatLoss))
        {
            if (block.Coords == lastBlockCoords)
            {
                return heatLoss;
            }

            foreach (var nextBlock in NextBlocks(block))
            {
                if (
                    nextBlock.Coords.Row < 0
                    || nextBlock.Coords.Row > city.LastRow
                    || nextBlock.Coords.Col < 0
                    || nextBlock.Coords.Col > city.LastCol
                )
                    continue;

                if (!visited.Contains(nextBlock))
                {
                    queue.Enqueue(nextBlock, heatLoss + city.Blocks[nextBlock.Coords]);
                    visited.Add(nextBlock);
                }
            }
        }

        return -1;
    }

    readonly List<Coords> Directions =
    [
        new(Row: 0, Col: 1),
        new(Row: 0, Col: -1),
        new(Row: 1, Col: 0),
        new(Row: -1, Col: 0),
    ];

    public IEnumerable<Block> NextBlocks(Block block)
    {
        foreach (var direction in Directions)
        {
            var newRow = block.Coords.Row + direction.Row;
            var newCol = block.Coords.Col + direction.Col;

            // Can't go back
            if (
                block.Direction != null
                && block.Direction.Row == direction.Row * -1
                && block.Direction.Col == direction.Col * -1
            )
                continue;

            // Can't go straight
            if (block.Direction != null && block.Direction == direction && block.Consecutive >= 3)
                continue;

            var newConsecutive = (block.Direction == direction) ? block.Consecutive + 1 : 1;
            yield return new Block(
                Coords: new Coords(Row: newRow, Col: newCol),
                Direction: direction,
                Consecutive: newConsecutive
            );
        }
    }

    public override string Part1()
    {
        var city = ParseInput();
        var lowestHeatLoss = FindLowestHeatLoss(city);
        return lowestHeatLoss.ToString();
    }

    public override string Part2() => "";
}
