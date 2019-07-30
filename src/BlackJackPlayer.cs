public class BlackJackPlayer : CasinoPlayer
{
    public readonly BlackJackHand Hand;

    public BlackJackPlayer(string name, int initialMoney) : base(name, initialMoney)
    {
        Hand = new BlackJackHand();
    }

    public override string ToString() => Name;

}