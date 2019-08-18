using System;

public class Player
{
    public readonly Hand Hand;
    public readonly string Name;
    public readonly int InitialMoney;
    private int _actualMoney;

    public Player(string name, int initialMoney) =>
        (Hand, Name, InitialMoney, _actualMoney) = (new Hand(), name, initialMoney, initialMoney);


    public int ActualMoney
    {
        get => _actualMoney;

        set
        {
            if (value < 0)
                throw new Exception("Cannot assign a negative number");
            _actualMoney = value;
        }
    }

    public int ActualBet
    {
        get => ActualBet;
        set
        {
            if (value < 0)
                throw new Exception("Cannot assign a negative number");
            ActualBet = value;
        }
    }

    public override string ToString() => Name;

}