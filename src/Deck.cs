using System;
using System.Collections.Generic;

namespace Cards
{

    public class Card : IComparable<Card>
    {
        public readonly string Name;
        public readonly char Suit;
        public int Value
        {
            get =>
                Name switch
                {
                    "ACE" => 11,
                    "TWO" => 2,
                    "THREE" => 3,
                    "FOUR" => 4,
                    "FIVE" => 5,
                    "SIX" => 6,
                    "SEVEN" => 7,
                    "EIGHT" => 8,
                    "NINE" => 9,
                    _ => 10
                };

        }
        public Card(string name, char suit) =>
            (Name, Suit) = (name, suit);

        public override string ToString() => $"{this.Name} of {this.Suit}";

        public int CompareTo(Card card) => this.Value.CompareTo(card.Value);

    }

    public class Deck
    {

        public static readonly char[] suits = { '♥', '♦', '♣', '♠' };
        public static readonly string[] Cards =
        { "ACE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" };
        private IList<Card> _deck;

        public Deck(int nDecks)
        {
            _deck = new List<Card>(52 * nDecks);
            for (int i = 0; i < nDecks; i++)
            {
                foreach (char suit in suits)
                {
                    foreach (string name in Cards)
                    {

                        _deck.Add(new Card(name, suit));

                    }
                }
            }
            Shuffle(_deck);
        }

        public IList<Card> GetInitialCards()
        {
            Card[] initialCards = { DealCard(), DealCard() };
            return new List<Card>(initialCards);
        }

        public Card DealCard()
        {
            Card card = _deck[0];
            _deck.RemoveAt(0);
            return card;
        }



        private static void Shuffle<T>(IList<T> items)
        {
            Random rand = new Random();

            // For each spot in the array, pick
            // a random item to swap into that spot.
            for (int i = 0; i < items.Count - 1; i++)
            {
                int j = rand.Next(i, items.Count);
                (items[i], items[j]) = (items[j], items[i]);
            }
        }
    }
}