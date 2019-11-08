#nullable enable

using System.Collections.Generic;

using Hands;
using Cards;

public class Player
{
    private Deck _deck;
    public Hand?[] Hands;
    public readonly string Name;
    public readonly int InitialMoney;
    public int Bet
    {
        get; set;
    }
    public int ActualMoney
    {
        get; set;
    }

    public Player(string name, int initialMoney, Deck deck) =>
        (Name, InitialMoney, ActualMoney, _deck, Hands) =
            (name, initialMoney, initialMoney, deck, new Hand?[] { new Hand(deck), null });


    public void ResetHands()
    {
        foreach (Hand? hand in Hands)
        {
            if (!(hand is null))
                hand.InitializeAttributes();
        }
    }

    public void Hit(int handIndex) => Hands[handIndex]!.DealCard();

    /// <returns>
    /// An optional error message
    /// </returns>
    public string? Double()
    {
        string? errorMessage;
        if (Bet * 2 > ActualMoney)
            errorMessage = "Cannot double because you have not enough money!";
        else if (Hands[0]!.Cards.Count != 2)
            errorMessage = "Cannot double because you have already hit!";
        else if (!(Hands[1] is null))
            errorMessage = "Cannot double because you have already splitted!";
        else
        {
            errorMessage = null;
            Bet *= 2;
        }
        return errorMessage;
    }

    /// <returns>
    /// An optional error message
    /// </returns>
    public string? Surrender()
    {
        string? errorMessage;
        if (Hands[0]!.Cards.Count != 2)
            errorMessage = "Cannot double because you have already hit!";
        else if (!(Hands[1] is null))
            errorMessage = "Cannot double because you have already splitted!";
        else
        {
            errorMessage = null;
            Bet /= 2;
            Hands[0]!.Points = 0;
        }

        return errorMessage;
    }

    /// <returns>
    /// An optional error message
    /// </returns>
    public string? Split()
    {
        string? errorMessage;
        IList<Card> firsHandCards = Hands[0]!.Cards;
        if (Bet * 2 > ActualMoney)
            errorMessage = "Cannot split because you have not enough money!";

        else if (!(Hands[1] is null))
            errorMessage = "Cannot split because you have already splitted!";

        else if (firsHandCards.Count != 2)
            errorMessage = "Cannot split because you have already hit!";

        else if (firsHandCards[0].Name != firsHandCards[1].Name)
            errorMessage = "Cannot split because your cards are not equal!";

        else
        {
            errorMessage = null;

            Bet *= 2;
            Card card = Hands[0]!.Cards[0];
            Hands[0]!.Cards.RemoveAt(0);
            Card[] cards = {
              card,
              _deck.DealCard()
            };
            Hands[1] = new Hand(this._deck, cards);

            Hands[0]!.DealCard();
        }
        return errorMessage;
    }

    public int Win()
    {
        int moneyBefore = ActualMoney;
        ActualMoney += Bet;

        // If has a BlackJack, sums 1.5 times the actual bet, otherwise just 1 time
        if (Hands[0]!.HasBlackJack())
            ActualMoney += Bet / 2;

        if (!(Hands[1] is null) && Hands[1]!.HasBlackJack())
            ActualMoney += Bet / 2;

        return ActualMoney - moneyBefore;
    }

    public void Lose()
    {
        ActualMoney -= Bet;
    }

    public override string ToString() => Name;

}