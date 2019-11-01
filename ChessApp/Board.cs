using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ChessApp
{
    public struct Point
    {
        public Point(char newX, int newY)
        {
            X = newX;
            Y = newY;
        }

        public char X { get; set; }
        public int Y { get; set; }
    }

    public static class Board
    {
        public static Dictionary<Point, Piece> board = new Dictionary<Point, Piece>();

        private const int BOARDSIZE = 8;
        public static void startUp()
        {
            board.Clear();

            char[] aToh = new char[BOARDSIZE] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

            for (int i = BOARDSIZE; i > 0; i--)
            {
                for (int j = 0; j < BOARDSIZE; j++)
                {
                    switch (i)
                    {
                        case 1:
                            lineSetup(aToh[j], i, PieceColour.Blue);
                            break;
                        case 2:
                            board.Add(new Point(aToh[j], i), new Piece(PieceColour.Blue, PieceType.Pawn));
                            break;
                        case 7:
                            board.Add(new Point(aToh[j], i), new Piece(PieceColour.Red, PieceType.Pawn));
                            break;
                        case 8:
                            lineSetup(aToh[j], i, PieceColour.Red);
                            break;
                        default:
                            board.Add(new Point(aToh[j], i), new Piece(PieceColour.Blank, PieceType.Blank));
                            break;
                    }
                    Point point = new Point(aToh[j], i);
                    //Console.WriteLine($"{point.X}{point.Y}: {board[point].colour} {board[point].type}");
                }
            }

            boardState();

        }

        private static void lineSetup(char lane, int position, PieceColour colour)
        {
            switch (lane)
            {
                case 'a':
                case 'h':
                    board.Add(new Point(lane, position), new Piece(colour, PieceType.Rook));
                    break;
                case 'b':
                case 'g':
                    board.Add(new Point(lane, position), new Piece(colour, PieceType.Knight));
                    break;
                case 'c':
                case 'f':
                    board.Add(new Point(lane, position), new Piece(colour, PieceType.Bishop));
                    break;
                case 'd':
                    board.Add(new Point(lane, position), new Piece(colour, PieceType.Queen));
                    break;
                case 'e':
                    board.Add(new Point(lane, position), new Piece(colour, PieceType.King));
                    break;
                default:
                    break;
            }
        }

        public static void boardState()
        {
            Console.BackgroundColor = ConsoleColor.White;

            char[] aToh = new char[BOARDSIZE] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            char[] oneToEight = new char[BOARDSIZE] { '1', '2', '3', '4', '5', '6', '7', '8' };

            string[] lineType = new string[BOARDSIZE];
            string[] lineColour = new string[BOARDSIZE];

            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("\n  a b c d e f g h  ");


            for (int i = BOARDSIZE; i > 0; i--)
            {
                for (int j = 0; j < BOARDSIZE; j++)
                {
                    Piece piece = board[new Point(aToh[j], i)];
                    switch (piece.type)
                    {
                        case PieceType.Blank:
                            lineType[j] = "  ";
                            lineColour[j] = piece.colour.ToString();
                            break;
                        case PieceType.Pawn:
                            lineType[j] = " P";
                            lineColour[j] = piece.colour.ToString();
                            break;
                        case PieceType.Rook:
                            lineType[j] = " R";
                            lineColour[j] = piece.colour.ToString();
                            break;
                        case PieceType.Knight:
                            lineType[j] = " N";
                            lineColour[j] = piece.colour.ToString();
                            break;
                        case PieceType.Bishop:
                            lineType[j] = " B";
                            lineColour[j] = piece.colour.ToString();
                            break;
                        case PieceType.Queen:
                            lineType[j] = " Q";
                            lineColour[j] = piece.colour.ToString();
                            break;
                        case PieceType.King:
                            lineType[j] = " K";
                            lineColour[j] = piece.colour.ToString();
                            break;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\n" + oneToEight[i - 1]);

                for (int k = 0; k < BOARDSIZE; k++)
                {
                    switch (lineColour[k])
                    {
                        case "Blue":
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case "Red":
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case "Blank":
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }

                    Console.Write(lineType[k]);
                }

                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" " + oneToEight[i - 1]);

            }

            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\n  a b c d e f g h  ");

            Console.ResetColor();
        }

        public static Point readPoint(char lane, char position)
        {
            int intPosition = (int)char.GetNumericValue(position);

            return new Point(lane, intPosition);
        }
    }
}
