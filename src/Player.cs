using System;

public class Player
{
    public Hand Hand = new Hand();
    public readonly string Name;
    public readonly int InitialMoney;
    private int _actualMoney;
    private int _actualBet = 0;

    public Player(string name, int initialMoney) =>
        (Name, InitialMoney, _actualMoney) = (name, initialMoney, initialMoney);


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
        get => _actualBet;
        set
        {
            if (value < 0)
                throw new Exception("Cannot assign a negative number");
            _actualBet = value;
        }
    }

    public override string ToString() => Name;

}