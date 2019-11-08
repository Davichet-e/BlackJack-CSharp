#nullable enable

using System;
using System.Collections.Generic;
using Cards;
using System.Linq;
namespace Hands
{


    public class Hand
    {
        private Deck _deck;
        public IList<Card> Cards
        {
            get; private set;
        }
        private int _aces;

        public int Points
        {
            get; set;
        }

        public Hand(Deck deck, IList<Card>? fromCards = null)
        {
            _deck = deck;
            if (fromCards is null)
                Cards = _deck.GetInitialCards();
            else
                Cards = fromCards;

            Points = CalculatePoints(Cards);
            _aces = 0;
            foreach (Card card in Cards)
                CheckIfAce(card);

            CheckAcePoints();
        }


        public void InitializeAttributes()
        {
            Cards = _deck.GetInitialCards();
            Points = CalculatePoints(Cards);
            _aces = 0;
            foreach (Card card in Cards)
                CheckIfAce(card);

            CheckAcePoints();

        }

        public bool HasBlackJack() => Cards.Count == 2 && Points == 21;

        public void DealCard()
        {
            Card card = _deck.DealCard();
            CheckIfAce(card);
            Cards.Add(card);
            updatePoints();
            if (Points > 21)
                Points = 0;
        }

        private void CheckIfAce(Card card)
        {
            if (card.Name.Equals("ACE"))
                _aces++;
        }


        private void CheckAcePoints()
        {
            while (Points > 21 && _aces > 0)
            {
                Points -= 10;
                _aces--;
            }
        }

        private void updatePoints()
        {
            Points = CalculatePoints(Cards);
            CheckAcePoints();
        }

        public override string ToString() =>
            $"{String.Join(", ", Cards)} ({(Points != 0 ? Points.ToString() : "> 21")} points)";

        public static int CalculatePoints(IList<Card> cards) => cards.Aggregate(0, (acc, card) => acc + card.Value);
    }
}