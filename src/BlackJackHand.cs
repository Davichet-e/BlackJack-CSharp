using System;
using System.Collections.Generic;
using Cards;

public class BlackJackHand
{
    readonly static Deck Deck = new Deck();
    private IList<Card> _cards;
    private int _points;
    private int _aces;

    public BlackJackHand()
    {
        InitializeAttributes();
    }

    public int Points { get => _points; }
    public IList<Card> Cards { get => _cards; }

    public void InitializeAttributes()
    {
        _cards = Deck.GetInitialCards();
        _points = CalculatePoints(_cards);
        foreach (Card card in _cards)
        {
            CheckIfAce(card);
        }
        CheckAcePoints();

    }

    public void DealCard()
    {
        Card card = Deck.DealCard();
        CheckIfAce(card);
        _cards.Add(card);
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
        while (_points > 21 && _aces > 0)
        {
            _points -= 10;
            _aces--;
        }
    }

    private void CheckIfLost()
    {
        if (_points > 21)
            _points = 0;
    }

    private void updatePoints(Card card)
    {
        _points += card.Value;
        CheckAcePoints();
    }

    public override string ToString() => String.Join(", ", _cards);

    public static int CalculatePoints(IList<Card> cards)
    {
        int res = 0;
        foreach (Card card in cards)
        {
            res += card.Value;
        }
        return res;
    }


}