using System.Text.RegularExpressions;

namespace Aoc2023.Day4;

class Day4 : Solution
{
    private record Card(int Number, List<int> CardNumbers, List<int> WinningNumbers);

    private static List<int> ParseNumbers(string s)
    {
        var parsedNumbers = new List<int> { };
        foreach (var n in s.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        {
            parsedNumbers.Add(int.Parse(n.Trim()));
        }
        return parsedNumbers;
    }

    private static List<Card> ParseCards(string[] lines)
    {
        var cards = new List<Card> { };

        foreach (var line in lines)
        {
            // Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            var lineSplit = line.Split(":");
            var cardNumberSplit = lineSplit
                .ElementAt(0)
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cardNumber = int.Parse(cardNumberSplit.ElementAt(1).Trim());
            var numbersSplit = lineSplit.ElementAt(1).Split("|");

            Card card =
                new(
                    cardNumber,
                    ParseNumbers(numbersSplit.ElementAt(0)),
                    ParseNumbers(numbersSplit.ElementAt(1))
                );
            cards.Add(card);
        }

        return cards;
    }

    public override string Part1()
    {
        var cards = ParseCards(Input);

        var sum = 0;
        foreach (var card in cards)
        {
            var score = 0;
            foreach (var n in card.CardNumbers)
            {
                if (card.WinningNumbers.Contains(n))
                {
                    score = (score == 0) ? 1 : score * 2;
                }
            }
            sum += score;
        }

        return sum.ToString();
    }

    public override string Part2()
    {
        var cards = ParseCards(Input);

        var copies = new Dictionary<int, int> { };
        foreach (var card in cards)
        {
            copies[card.Number] = 1;
        }

        for (var i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var winning = 0;
            foreach (var n in card.CardNumbers)
            {
                if (card.WinningNumbers.Contains(n))
                {
                    winning += 1;
                }
            }

            for (var j = cards[i].Number + 1; j <= cards[i].Number + winning; j++)
            {
                copies[j] += copies[card.Number];
            }
        }

        return copies.Values.Sum().ToString();
    }
}
