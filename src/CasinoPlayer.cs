using System;

public class CasinoPlayer
{
    public readonly string Name;
    public readonly int InitialMoney;
    private int _actualMoney;

    public CasinoPlayer(string name, int initialMoney) =>
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

    public int ActualBet { get; set; }




}