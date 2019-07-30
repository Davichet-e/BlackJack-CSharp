using System;
using System.Collections.Generic;

namespace Cards
{

    public class Card : IComparable<Card>
    {
        public readonly string Name;
        public readonly string Suit;
        public readonly int Value;
        public Card(string name, string suit, int value)
            => (Name, Suit, Value) = (name, suit, value);

        public override String ToString() => $"{this.Name} of {this.Suit}";

        public int CompareTo(Card card) => this.Value.CompareTo(card.Value);

    }

    public class Deck
    {

        private IList<Card> _deck = new List<Card>(52);


        public static readonly string[] suits = { "HEARTS", "DIAMONDS", "CLUBS", "PIKES" };
        public static readonly Dictionary<string, int> Cards = new Dictionary<string, int>() {
            { "ACE", 11 },
            { "TWO", 2 },
            { "THREE", 3 },
            { "FOUR", 4 },
            { "FIVE", 5 },
            { "SIX", 6 },
            { "SEVEN", 7 },
            { "EIGHT", 8 },
            { "NINE", 9 },
            { "TEN", 10 },
            { "JACK", 10 },
            { "QUEEN", 10 },
            { "KING", 10 }
        };

        public Deck()
        {
            foreach (string suit in suits)
            {
                foreach (KeyValuePair<string, int> entry in Cards)
                {

                    _deck.Add(new Card(entry.Key, suit, entry.Value));

                }
            }
            Shuffle(_deck);
        }
        public IList<Card> GetInitialCards()
        {
            Card[] initialCards = { _deck[0], _deck[1] };
            _deck.RemoveAt(0);
            _deck.RemoveAt(1);
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
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }
    }
}