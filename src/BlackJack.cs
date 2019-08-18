using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Main
{

    public class BlackJack
    {
        private static IList<Player> _players = new List<Player>();
        private static readonly Hand _dealerHand = new Hand();

        public static void Main(string[] args)
        {
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
            int numberOfPeople = 0;
            while (true)
            {
                try
                {
                    Console.WriteLine("How many people are going to play? (1-5)");

                    numberOfPeople = int.Parse(Console.ReadLine());

                    if (!(0 < numberOfPeople) && (numberOfPeople <= 5))
                        Console.WriteLine("The number of people must be between 1 and 5\n");
                    else
                        break;
                }
                catch (Exception)
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
                Console.WriteLine($"Please, enter your name, Player {i}");

                string name = Console.ReadLine();

                while (true)
                {
                    try
                    {
                        Console.WriteLine("How much money do you have? (Use only numbers)");
                        int initialMoney = int.Parse(Console.ReadLine());

                        if (initialMoney < 50)
                            Console.WriteLine("The initial money must be greater or equal than 50\n");

                        else
                        {
                            _players.Add(new Player(name, initialMoney));
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
                    Console.WriteLine("What bet do you wanna make?");

                    int bet = int.Parse(Console.ReadLine());
                    if (bet > player.ActualMoney)
                        Console.WriteLine("Your bet cannot be greater than your actual money.\n");
                    else if (bet <= 0)
                        Console.WriteLine("Your bet must be greater than 0.\n");
                    else
                    {
                        player.ActualBet = bet;
                        break;
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Please, use only integral values.\n");
                }

            }
        }

        private static bool PlayerWinOrLose(Player player)
        {
            bool result = false;

            int playerPoints = player.Hand.Points;

            if (playerPoints == 21)
            {
                Console.WriteLine("BLACKJACK");
                result = true;
            }
            else if (playerPoints == 0)
            {
                Console.WriteLine("BUST.\nI'm afraid you lose this game :(\n");
                result = true;
            }

            return result;
        }

        private static bool CheckIfYes(string userDecision)
        {
            string[] positiveAnswers = { "y", "yes", "1", "true" };
            return Array.IndexOf(positiveAnswers, userDecision.Trim().ToLower()) != -1;
        }

        private static bool AskIfHit()
        {
            Console.WriteLine("Do you wanna hit? (y/n)");
            string decision = Console.ReadLine();
            return CheckIfYes(decision);
        }

        private static void PlayerTurn(Player player)
        {
            //TODO console.wr(player turn)
            Console.WriteLine($"{player}, your actual money is {player.ActualMoney} Euros\n");
            AskPlayerBet(player);

            Console.WriteLine("Your cards are: ");
            Console.WriteLine($"{player.Hand.Cards[0]} and {player.Hand.Cards[1]}");

            Thread.Sleep(1000);

            while (!PlayerWinOrLose(player))
            {
                bool hit = AskIfHit();
                if (hit)
                {
                    player.Hand.DealCard();
                    Console.WriteLine($"Now, your cards are: {player.Hand}");
                    Thread.Sleep(1000);

                }
                else
                {
                    Console.WriteLine($"{player.Name} stood\n");
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
            //TODO Console.WriteLine(Dealer's Turn)
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
                int playerPoints = player.Hand.Points;
                if (playerPoints == 21 || playerPoints > dealerPoints)
                {
                    player.ActualMoney += player.ActualBet;
                    Console.WriteLine($"{player} won {player.ActualBet * 2} Euros :)\n");
                }
                else if (playerPoints == 0 || playerPoints < dealerPoints)
                {
                    player.ActualMoney -= player.ActualBet;
                    Console.WriteLine($"{player} lost against the dealer :(\n");
                }
                else
                    Console.WriteLine($"{player}, it is a Tie! :|\n");
            }
            Thread.Sleep(1000);
        }

        private static bool AskIfNextGame(Player player)
        {
            bool playerNextGame = false;
            string finalBalance = $"{player.ActualMoney - player.InitialMoney} Euros";
            if (!finalBalance.Contains("-"))
            {
                finalBalance = "+" + finalBalance;
            }

            if (player.ActualMoney > 0)
            {
                Console.WriteLine($"{player}, do you want to play again? (y/n)");
                string decision = Console.ReadLine();

                if (CheckIfYes(decision))
                {
                    player.Hand.InitializeAttributes();
                    playerNextGame = true;
                }
                else
                    Console.WriteLine($"Thanks for playing {player}, your final balance is {finalBalance}\n");
            }
            else
                Console.WriteLine($"{player}, you have lost all your money. Thanks for playing\n");

            return playerNextGame;
        }

        private static bool NextGame()
        {
            //TODO Console.WriteLine(Game Finished)
            _players = _players.Where(AskIfNextGame).ToList();

            if (_players.Count != 0)
            {
                _dealerHand.InitializeAttributes();
                return true;
            }

            return false;
        }



    }
}