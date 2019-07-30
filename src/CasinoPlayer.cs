using System;

public class CasinoPlayer
{
    public readonly string Name;
    public readonly int InitialMoney;
    private int _actualMoney;
    private int _actualBet;

    public CasinoPlayer(string name, int initialMoney) =>
        (Name, InitialMoney, _actualMoney, _actualBet) = (name, initialMoney, initialMoney, 0);


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




}