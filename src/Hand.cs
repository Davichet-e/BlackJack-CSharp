using System.Collections.Generic;
using Cards;
using System.Linq;

public class Hand
{
    public readonly static Deck Deck = new Deck();

    public readonly IList<Card> Cards = Deck.GetInitialCards();

    private int _aces;

    public Hand()
    {
        InitializeAttributes();
    }

    public int Points
    {
        get; private set;
    }

    public void InitializeAttributes()
    {
        Points = CalculatePoints(Cards);
        foreach (Card card in Cards)
        {
            CheckIfAce(card);
        }
        CheckAcePoints();

    }

    public void DealCard()
    {
        Card card = Deck.DealCard();
        CheckIfAce(card);
        Cards.Add(card);
        updatePoints(card);
        CheckIfLost();
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

    private void CheckIfLost()
    {
        if (Points > 21)
            Points = 0;
    }

    private void updatePoints(Card card)
    {
        Points += card.Value;
        CheckAcePoints();
    }

    public override string ToString() => string.Join(", ", Cards);

    public static int CalculatePoints(IList<Card> cards)
    {
        return cards.Aggregate(0, (acc, card) => acc + card.Value);
    }


}