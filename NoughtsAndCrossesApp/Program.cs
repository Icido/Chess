using System;

namespace NoughtsAndCrossesApp
{
    class Program
    {
        static bool isPlayerCross = true;
        static bool playersTurn = true;
        static bool shutDown = false;
        static char inputChar;

        static void Main(string[] args)
        {
            playerStartUp();
            gameState();
        }

        static void playerStartUp()
        {
            Console.WriteLine("\nSetting up board!");
            Board.startUp();

            Random coinFlip = new Random();
            if (coinFlip.NextDouble() < 0.5d)
            {
                Console.WriteLine("Player goes first!");
                playersTurn = true;
            }
            else
            {
                Console.WriteLine("Computer goes first!");
                playersTurn = false;
            }
        }

        static void gameState()
        {
            while (!shutDown)
            {
                if (playersTurn)
                {
                    Console.WriteLine("Player's turn...");

                    //Console.WriteLine($"Optimal move is: {Computer.TupleToMove(new StateNode().miniMax(Board.board, true))}");

                    inputChar = Console.ReadKey().KeyChar;

                    if (char.ToUpper(inputChar) == 'C')
                    {
                        Board.boardState();
                        continue;
                    }
                    else if (char.ToUpper(inputChar) == 'S')
                    {
                        playersTurn = false;
                        continue;
                    }
                    else if (char.ToUpper(inputChar) == 'Q')
                    {
                        Console.WriteLine("\nShutting down...");
                        shutDown = true;
                    }
                    else if (char.ToUpper(inputChar) == 'R')
                    {
                        Console.WriteLine("\nResetting...");
                        playerStartUp();
                        continue;
                    }

                    if (Logic.CheckMove(inputChar))
                    {
                        Console.WriteLine($"\nSelected {inputChar}.");
                        Logic.ExecuteMove('X');
                        playersTurn = false;
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid input: " + Logic.errMsg);
                    }
                }
                else
                {
                    Console.WriteLine("Computer's turn...");

                    while(true)
                    {
                        inputChar = char.Parse(Computer.minimax().ToString());

                        if (Logic.CheckMove(inputChar))
                        {
                            Console.WriteLine($"\nSelected {inputChar}.");
                            Logic.ExecuteMove('O');
                            playersTurn = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid input: " + Logic.errMsg);
                        }

                        break;
                    }
                }

                if(Logic.WinCheck())
                {

                    Board.boardState();

                    if (!Logic.isDraw)
                    {
                        if ((Logic.isCrossWinner && isPlayerCross) ||
                            (!Logic.isCrossWinner && !isPlayerCross))
                        {
                            Console.WriteLine("Congratulations! You win!");
                        }
                        else
                        {
                            Console.WriteLine("Sorry! You lose!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Board full - No winners.");
                    }

                    Console.WriteLine("Would you like to play again? Y/N");

                    while (true)
                    {
                        inputChar = Console.ReadKey().KeyChar;

                        if (char.ToUpper(inputChar) == 'Y')
                        {
                            playerStartUp();
                            break;
                        }
                        else if (char.ToUpper(inputChar) == 'N' || char.ToUpper(inputChar) == 'Q')
                        {
                            Console.WriteLine("\nShutting down...");
                            shutDown = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\nIncorrect entry, please try again.");
                        }
                    }
                }

                Board.boardState();
            }
        }
    }
}
