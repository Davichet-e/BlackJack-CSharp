using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

using Hands;
using Cards;

#pragma warning disable CS8618
public class BlackJack
{


    private static Deck _deck;
    private static IList<Player> _players = new List<Player>();
    private static Hand _dealerHand;

    public static void Main(string[] args)
    {
        Console.WriteLine("This BlackJack Game has been created by David Garcia Morillo");
        int nOfDecks;
        while (true)
        {
            try
            {
                Console.Write("How many decks do you want to use (4-8)\n> ");
                nOfDecks = Int32.Parse(Console.ReadLine());
                if (nOfDecks <= 3 || nOfDecks > 8)
                    Console.WriteLine("The number of decks must be between 4 and 8\n");
                else
                    break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Please, use only integral values");
            }
        }
        _deck = new Deck(nOfDecks);
        _dealerHand = new Hand(_deck);

        StartGame();
        while (true)
        {
            //TODO console.wr(game started)
            Console.WriteLine($"\nThe first card of the dealer is {_dealerHand.Cards[0]}");

            foreach (Player player in _players)
            {
                PlayerTurn(player);
            }

            DealerTurn();
            EndGame();
            if (!NextGame())
            {
                break;
            }
        }
    }

    private static void StartGame()
    {
        int numberOfPeople = AskNumberOfPeople();
        AskAndSetPlayerAttributes(numberOfPeople);
    }

    private static int AskNumberOfPeople()
    {
        int numberOfPeople;
        while (true)
        {
            try
            {
                Console.Write("How many people are going to play? (1-5)\n> ");

                numberOfPeople = int.Parse(Console.ReadLine());

                if (!(0 < numberOfPeople) && (numberOfPeople <= 5))
                    Console.WriteLine("The number of people must be between 1 and 5\n");
                else
                    break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Please, use only integral values.\n");
            }
        }
        return numberOfPeople;
    }

    private static void AskAndSetPlayerAttributes(int numberOfPeople)
    {
        for (int i = 1; i <= numberOfPeople; i++)
        {
            Console.Write($"Please, enter your name, Player {i}\n> ");

            string name = Console.ReadLine();

            while (true)
            {
                try
                {
                    Console.Write("\nHow much money do you have? (Use only integral values)\n> ");
                    int initialMoney = int.Parse(Console.ReadLine());

                    if (initialMoney < 50)
                        Console.WriteLine("The initial money must be greater or equal than 50\n");

                    else
                    {
                        _players.Add(new Player(name, initialMoney, _deck));
                        break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please, use only integral values.\n");
                }
            }
        }
    }

    private static void AskPlayerBet(Player player)
    {
        while (true)
        {
            try
            {
                Console.Write("What bet do you wanna make?\n> ");

                int bet = int.Parse(Console.ReadLine());
                if (bet > player.ActualMoney)
                    Console.WriteLine("Your bet cannot be greater than your actual money.\n");
                else if (bet <= 0)
                    Console.WriteLine("Your bet must be greater than 0.\n");
                else
                {
                    player.Bet = bet;
                    break;
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Please, use only integral values.\n");
            }

        }
    }

    private static bool HandWinOrLose(Hand hand)
    {
        bool result;
        int playerPoints = hand.Points;

        if (playerPoints == 21)
        {
            if (hand.HasBlackJack())
                Console.WriteLine("BLACKJACK!");
            else
                Console.WriteLine("YOU GOT 21 POINTS!");
            result = true;
        }
        else if (playerPoints == 0)
        {
            Console.WriteLine("BUST.\nI'm afraid you lose this game :(\n");
            result = true;
        }
        else
            result = false;

        return result;
    }


    private static bool CheckIfYes(string userDecision)
    {
        string[] positiveAnswers = { "y", "yes", "1", "true" };
        return positiveAnswers.Contains(userDecision.Trim().ToLower());
    }

    private static bool AskIfHit()
    {
        Console.Write("Do you wanna hit? (y/n)\n> ");
        string decision = Console.ReadLine();
        return CheckIfYes(decision);
    }


    private static void PlayerTurn(Player player)
    {
        Console.WriteLine($"###### {player}'s turn ######\n");
        Console.WriteLine($"{player}, your actual money is {player.ActualMoney} Euros\n");
        AskPlayerBet(player);

        Console.WriteLine("Your cards are: ");
        Console.WriteLine(String.Join(" and ", player.Hands[0]!.Cards));

        Thread.Sleep(1000);

        bool hasSplitted = false;
        bool hasDoubled = false;
        foreach ((Hand hand, int i) in player.Hands.Select((i, hand) => (i, hand)))
        {
            if (hand is null)
                break;

            // If the player has doubled, he can only hit one more time
            while (!HandWinOrLose(hand) && (!hasDoubled || hand.Cards.Count < 3))
            {
                if (hasSplitted)
                {
                    Console.WriteLine($"(Hand #{i})");
                    Console.WriteLine($"Your cards are: {hand}");
                }

                Console.Write("\nWhat do you want to do?\nAvailable Commands: (h)it, (s)tand, (sp)lit, (d)ouble, (surr)ender\n> ");

                string userDecision = Console.ReadLine().Trim().ToLower();

                bool breaking = false;
                switch (userDecision)
                {
                    case "h":
                    case "hit":
                        player.Hit(i);
                        Console.WriteLine($"Now, your cards are: {hand}");
                        break;

                    case "s":
                    case "stand":
                        Console.WriteLine($"Player ${player} stood.");
                        breaking = true;
                        break;

                    case "sp":
                    case "split":
                        if (!hasDoubled)
                        {
                            string? errorMessage = player.Split();
                            if (!string.IsNullOrEmpty(errorMessage))
                                Console.WriteLine(errorMessage);
                            else
                            {
                                hasSplitted = true;
                                Console.WriteLine("You have splitted the hand!");
                            }
                        }
                        else
                            Console.WriteLine("You cannot split because you have already doubled");
                        break;

                    case "d":
                    case "doubled":
                        if (!hasDoubled)
                        {
                            string? errorMessage = player.Double();
                            if (!string.IsNullOrEmpty(errorMessage))
                                Console.WriteLine(errorMessage);
                            else
                            {
                                hasDoubled = true;
                                Console.WriteLine("You have doubled the bet!");
                            }
                        }
                        else
                            Console.WriteLine("You cannot double because you have already doubled");
                        break;

                    case "surr":
                    case "surrender":
                        if (!hasDoubled)
                        {
                            string? errorMessage = player.Surrender();
                            if (!string.IsNullOrEmpty(errorMessage))
                                Console.WriteLine(errorMessage);
                            else
                            {
                                Console.WriteLine("You have surrendered!");
                                breaking = true;
                            }
                        }
                        else
                            Console.WriteLine("You cannot surrender because you have already doubled");
                        break;
                    default:
                        Console.WriteLine("Invalid command!\nAvailable Commands: (h)it, (s)tand, (sp)lit, (d)ouble, (surr)ender");
                        break;
                }
                if (breaking)
                    break;

            }
        }
    }
    private static bool DealerLost()
    {
        if (_dealerHand.Points == 0)
        {
            Console.WriteLine("The dealer busted. The game ended :)\n");
            return true;
        }
        return false;
    }

    private static void DealerTurn()
    {
        Console.WriteLine("###### Dealer's Turn ######\n");
        Thread.Sleep(2000);
        Console.WriteLine($"The dealer cards are {_dealerHand.Cards[0]} and {_dealerHand.Cards[1]}\n");

        while (!DealerLost() && _dealerHand.Points < 17)
        {
            Thread.Sleep(2000);
            Console.WriteLine("The dealer is going to hit a card\n");
            _dealerHand.DealCard();
            Thread.Sleep(1000);
            Console.WriteLine($"Now, the dealer cards are: {_dealerHand}");
        }
    }

    private static void EndGame()
    {
        // TODO Console.WriteLine(Results)
        int dealerPoints = _dealerHand.Points;


        foreach (Player player in _players)
        {
            foreach ((Hand hand, int i) in player.Hands.Select((i, hand) => (i, hand)))
            {
                if (hand is null)
                    break;

                int handPoints = hand.Points;
                if (handPoints > _dealerHand.Points ||
                    (hand.HasBlackJack() && !_dealerHand.HasBlackJack()))
                {
                    int moneyEarned = player.Win();
                    string handSpecification = player.Hands.Length == 1 ? "" : $" (#{i + 1} hand)";

                    Console.WriteLine($"\n{player}{handSpecification} won {moneyEarned} Euros :)\n");
                }
                else if (handPoints == 0 || handPoints < dealerPoints)
                {
                    player.Lose();
                    Console.WriteLine($"\n{player} lost against the dealer :(\n");
                }
                else
                    Console.WriteLine($"\n{player}, it is a Tie! :|\n");
            }
            Thread.Sleep(1000);
        }
    }

    private static bool AskIfReset(Player player)
    {
        bool playerResets = false;
        string finalBalance = $"{player.ActualMoney - player.InitialMoney} Euros";
        if (!finalBalance.Contains("-"))
        {
            finalBalance = "+" + finalBalance;
        }

        if (player.ActualMoney > 0)
        {
            Console.Write($"{player}, do you want to play again? (y/n)\n> ");
            string decision = Console.ReadLine();

            if (CheckIfYes(decision))
            {
                player.ResetHands();
                playerResets = true;
            }
            else
                Console.WriteLine($"Thanks for playing {player}, your final balance is {finalBalance}\n");
        }
        else
            Console.WriteLine($"{player}, you have lost all your money. Thanks for playing\n");

        return playerResets;
    }

    private static bool NextGame()
    {
        //TODO Console.WriteLine(Game Finished)
        _players = _players.Where(AskIfReset).ToList();

        if (_players.Count != 0)
        {
            _dealerHand.InitializeAttributes();
            return true;
        }

        return false;
    }



}
