using System.Data;
using System.Text.RegularExpressions;

namespace Aoc2023.Day7;

class Day7 : Solution
{
    private List<Bid> ParseInput(bool applyJokers, Dictionary<char, int> strenghts)
    {
        var bids = new List<Bid>();
        foreach (var line in Input)
        {
            var lineSplit = line.Split();
            var amount = int.Parse(lineSplit.ElementAt(1));

            var original = lineSplit.ElementAt(0);
            var variants = new HashSet<string>();

            if (applyJokers)
            {
                var mostCommon = original
                    .Where(c => c != 'J')
                    .GroupBy(c => c)
                    .OrderByDescending(g => g.Count());
                if (mostCommon.Count() == 0) // "JJJJJ"
                    variants.Add(original);
                else
                    variants.Add(original.Replace('J', mostCommon.First().Key));
            }
            else
                variants.Add(original);

            var hands = new List<Hand>();
            foreach (var variant in variants)
                hands.Add(new Hand(SymbolsToCards(variant, original, strenghts)));

            bids.Add(new Bid(amount, hands));
        }
        return bids;
    }

    private List<Card> SymbolsToCards(
        string symbols,
        string original,
        Dictionary<char, int> strenghts
    )
    {
        var cards = new List<Card>();
        for (var i = 0; i < symbols.Length; i++)
            cards.Add(new Card(symbols[i], strenghts[original[i]]));
        return cards;
    }

    public override string Part1()
    {
        var strenghts = new Dictionary<char, int>
        {
            { '2', 0 },
            { '3', 1 },
            { '4', 2 },
            { '5', 3 },
            { '6', 4 },
            { '7', 5 },
            { '8', 6 },
            { '9', 7 },
            { 'T', 8 },
            { 'J', 9 },
            { 'Q', 10 },
            { 'K', 11 },
            { 'A', 12 },
        };
        var bids = ParseInput(false, strenghts);
        bids.Sort();
        return bids.Order().Select((bid, i) => (i + 1) * bid.Amount).Sum().ToString();
    }

    public override string Part2()
    {
        var strenghts = new Dictionary<char, int>
        {
            { 'J', 0 },
            { '2', 1 },
            { '3', 2 },
            { '4', 3 },
            { '5', 4 },
            { '6', 5 },
            { '7', 6 },
            { '8', 7 },
            { '9', 8 },
            { 'T', 9 },
            { 'Q', 10 },
            { 'K', 11 },
            { 'A', 12 },
        };
        var bids = ParseInput(true, strenghts);
        return bids.Order().Select((bid, i) => (i + 1) * bid.Amount).Sum().ToString();
    }

    record Card(char Symbol, int Strength);

    record Hand(List<Card> Cards)
    {
        public int Rank
        {
            get
            {
                if (IsFiveOfAKind())
                    return 7;
                if (IsFourOfAKind())
                    return 6;
                if (IsFullHouse())
                    return 5;
                if (IsThreeOfAKind())
                    return 4;
                if (IsTwoPair())
                    return 3;
                if (IsOnePair())
                    return 2;
                if (IsHighCard())
                    return 1;
                return 0;
            }
        }

        List<int> DistinctCounts() =>
            Cards.GroupBy(c => c.Symbol).Select(g => g.Count()).Order().ToList();

        bool IsFiveOfAKind() => DistinctCounts().SequenceEqual(new List<int> { 5 });

        bool IsFourOfAKind() => DistinctCounts().SequenceEqual(new List<int> { 1, 4 });

        bool IsFullHouse() => DistinctCounts().SequenceEqual(new List<int> { 2, 3 });

        bool IsThreeOfAKind() => DistinctCounts().SequenceEqual(new List<int> { 1, 1, 3 });

        bool IsTwoPair() => DistinctCounts().SequenceEqual(new List<int> { 1, 2, 2 });

        bool IsOnePair() => DistinctCounts().SequenceEqual(new List<int> { 1, 1, 1, 2 });

        bool IsHighCard() => DistinctCounts().SequenceEqual(new List<int> { 1, 1, 1, 1, 1 });
    }

    record Bid(int Amount, List<Hand> Hands) : IComparable<Bid>
    {
        public int CompareTo(Bid? other)
        {
            if (other == null)
                return 1;

            if (this.MaxHand.Rank > other.MaxHand.Rank)
                return 1;
            else if (this.MaxHand.Rank == other.MaxHand.Rank)
                return CompareCards(this.MaxHand.Cards, other.MaxHand.Cards);
            else
                return -1;
        }

        Hand MaxHand
        {
            get => Hands.MaxBy(h => h.Rank)!;
        }

        int CompareCards(List<Card> a, List<Card> b)
        {
            for (var i = 0; i < a.Count; i++)
            {
                if (a[i].Strength > b[i].Strength)
                    return 1;
                else if (a[i].Strength < b[i].Strength)
                    return -1;
            }
            return 0;
        }
    }
}
