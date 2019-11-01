using System;

namespace ChessApp
{
    class Program
    {
        //Blue == White || Red == Black
        static bool isPlayerOneBlue = false;
        static bool playersTurn = false;
        static bool pvp = false;
        static string inputString = "";
        static PieceColour colour;

        private static void Main(string[] args)
        {
            char input;

            while (true)
            {
                Console.WriteLine("Would you like to play again? y/n");

                input = char.ToLower(Console.ReadKey().KeyChar);

                if (char.ToLower(input) == 'y')
                {
                    playerSetup();
                    if (pvp)
                        pvpGameLoop();
                    else
                        gameLoop();
                }
                else if (char.ToLower(input) == 'n')
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nIncorrect entry, please try again.");
                }
            }
        }

        private static void playerSetup()
        {
            Console.WriteLine("\nSetting up board!");
            Board.startUp();

            Console.WriteLine("\nNote: Move entry is as such: a1b1");
            Console.WriteLine("\nWhere a1 is the piece to move and b1 is the end position of the piece");

            char input;

            while (true)
            {
                Console.WriteLine("\nVs Computer or Vs Player?:");
                Console.WriteLine("\nPress 'p' for Player or 'c' for Computer");
                input = char.ToLower(Console.ReadKey().KeyChar);

                if (char.ToLower(input) == 'p')
                {
                    Console.WriteLine("\nPlayer vs. Player.");
                    pvp = true;
                    break;
                }
                else if (char.ToLower(input) == 'c')
                {
                    Console.WriteLine("\nPlayer vs. Computer.");
                    pvp = false;
                    break;
                }
                else
                {
                    Console.WriteLine("\nIncorrect entry, please try again.");
                }
            }


            while (true)
            {
                Console.WriteLine("\nPlease choose which colour to play:");
                Console.WriteLine("\nPress 'b' for Blue or 'r' for Red");
                input = char.ToLower(Console.ReadKey().KeyChar);

                if (input == 'b')
                {
                    Console.WriteLine("\nPlayer plays Blue. Player goes first.");
                    isPlayerOneBlue = true;
                    playersTurn = true;
                    break;
                }
                else if (input == 'r')
                {
                    Console.WriteLine("\nPlayer plays Red. Computer goes first.");
                    isPlayerOneBlue = false;
                    playersTurn = false;
                    break;
                }
                else
                {
                    Console.WriteLine("\nIncorrect entry, please try again.");
                }
            }
        }

        private static void gameLoop()
        {
            while (true)
            {
                Board.boardState();

                if (playersTurn)
                {
                    Console.ForegroundColor = (isPlayerOneBlue) ? ConsoleColor.Blue : ConsoleColor.Red;
                    colour = isPlayerOneBlue ? PieceColour.Blue : PieceColour.Red;
                    Console.WriteLine("\nPlayer's turn...");

                    if (Logic.KingCheck(colour))
                        Console.WriteLine("\nPlayer is now in check.");

                    if (playerMove(colour))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.ForegroundColor = (!isPlayerOneBlue) ? ConsoleColor.Blue : ConsoleColor.Red;
                    colour = !isPlayerOneBlue ? PieceColour.Blue : PieceColour.Red;
                    Console.WriteLine("\nComputer's turn...");

                    if (Logic.KingCheck(colour))
                        Console.WriteLine("\nComputer is now in check.");

                    Computer computer = new Computer();
                    computer.computerMove(colour);

                    playersTurn = !playersTurn;

                    if (Logic.isGameOver(colour))
                        break;

                }
            }
        }

        private static void pvpGameLoop()
        {
            while (true)
            {
                Board.boardState();

                if (playersTurn)
                {
                    Console.ForegroundColor = (isPlayerOneBlue) ? ConsoleColor.Blue : ConsoleColor.Red;
                    colour = isPlayerOneBlue ? PieceColour.Blue : PieceColour.Red;
                    Console.WriteLine("\nPlayer 1's turn...");

                    if (Logic.KingCheck(colour))
                        Console.WriteLine("\nPlayer 1 is now in check.");

                    if (playerMove(colour))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.ForegroundColor = (!isPlayerOneBlue) ? ConsoleColor.Blue : ConsoleColor.Red;
                    colour = !isPlayerOneBlue ? PieceColour.Blue : PieceColour.Red;
                    Console.WriteLine("\nPlayer 2's turn...");

                    if (Logic.KingCheck(colour))
                        Console.WriteLine("\nPlayer 2 is now in check.");

                    if (playerMove(colour))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static bool playerMove(PieceColour colour)
        {
            inputString = Console.ReadLine().ToLower();

            switch (inputString)
            {
                case "check board":
                    Board.boardState();
                    break;
                case "skip":
                    playersTurn = !playersTurn;
                    break;
                case "quit":
                    Console.WriteLine("\nShutting down...");
                    return false;
                case "reset":
                    Console.WriteLine("\nResetting...");
                    return false;
                default:
                    break;
            }

            if (inputString.Length != 4)
            {
                Console.WriteLine("\nInvalid entry, please try again.");
                return true;
            }

            char[] inputChar = inputString.ToCharArray();
            Point pieceToMove = Board.readPoint(char.ToLower(inputChar[0]), inputChar[1]);
            Point positionToMoveTo = Board.readPoint(char.ToLower(inputChar[2]), inputChar[3]);

            if (Logic.CheckMove(colour, pieceToMove, positionToMoveTo))
            {
                Piece pieceToMovePiece = Board.board[pieceToMove];
                Piece positionToMoveToPiece = Board.board[positionToMoveTo];

                Logic.ExecuteMove(pieceToMove, positionToMoveTo);

                if(Logic.KingCheck(colour))
                {
                    Console.WriteLine("\nCannot move piece, will cause King to be in Check.");
                    Logic.Reverse();
                    return true;
                }

                Console.WriteLine($"Moved {Board.board[positionToMoveTo].type} from {pieceToMove.X}{pieceToMove.Y} to {positionToMoveTo.X}{positionToMoveTo.Y}");
                playersTurn = !playersTurn;

                if (Logic.isGameOver(colour))
                    return false;
            }
            else
            {
                Console.WriteLine("\nInvalid move, please try again.");
                return true;
            }

            return true;
        }
    }
}
