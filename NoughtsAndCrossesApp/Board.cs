using System;
using System.Collections.Generic;
using System.Text;

namespace NoughtsAndCrossesApp
{
    public static class Board
    {
        public static char[,] board = new char[3, 3];
        private const int BOARDSIZE = 9;
        private const int BOARDLENGTH = 3;

        public static void startUp()
        {
            for (int i = 0; i < BOARDLENGTH; i++)
            {
                for (int j = 0; j < BOARDLENGTH; j++)
                {
                    board[i, j] = char.Parse($"{(i * BOARDLENGTH) + j + 1}");
                }
            }

            boardState();
        }

        public static void boardState()
        {
            Console.WriteLine("\nBoard state:");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < BOARDLENGTH; i++)
            {
                for (int j = 0; j < BOARDLENGTH; j++)
                {
                    Console.Write(board[i, j] + " ");
                }

                Console.WriteLine();
            }

            Console.ResetColor();
        }
    }
}
