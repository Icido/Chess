using System;
using System.Collections.Generic;
using System.Text;

namespace NoughtsAndCrossesApp
{
    public static class Logic
    {
        public static int currentInput;
        private static Tuple<int, int> currentMove = new Tuple<int, int>(-1, -1);
        public static string errMsg = "";
        public static bool isCrossWinner = false;
        public static bool isPlayerWinner = false;
        public static bool isDraw = false;

        private const int BOARDSIZE = 9;
        private const int BOARDLENGTH = 3;

        public static bool CheckMove(char inputMove)
        {
            currentInput = (int)char.GetNumericValue(inputMove);

            switch (currentInput)
            {
                case 1:
                    currentMove = new Tuple<int, int>(0, 0);
                    break;
                case 2:
                    currentMove = new Tuple<int, int>(0, 1);
                    break;
                case 3:
                    currentMove = new Tuple<int, int>(0, 2);
                    break;
                case 4:
                    currentMove = new Tuple<int, int>(1, 0);
                    break;
                case 5:
                    currentMove = new Tuple<int, int>(1, 1);
                    break;
                case 6:
                    currentMove = new Tuple<int, int>(1, 2);
                    break;
                case 7:
                    currentMove = new Tuple<int, int>(2, 0);
                    break;
                case 8:
                    currentMove = new Tuple<int, int>(2, 1);
                    break;
                case 9:
                    currentMove = new Tuple<int, int>(2, 2);
                    break;
                default:
                    errMsg = "Outside of board scope.";
                    return false;
            }


            switch (Board.board[currentMove.Item1, currentMove.Item2])
            {
                case 'X':
                case 'O':
                    errMsg = "Position already filled.";
                    return false;
                default:
                    return true;
            }
        }

        public static void ExecuteMove(char isCross)
        {
            Board.board[currentMove.Item1, currentMove.Item2] = isCross;
        }

        public static bool WinCheck()
        {
            int xCount = 0;
            int oCount = 0;
            List<char> rowList = new List<char>();
            List<char> columnList = new List<char>();

            isDraw = false;

            foreach (var item in Board.board)
            {
                if(item == 'O')
                {
                    oCount++;
                }
                else if(item == 'X')
                {
                    xCount++;
                }
            }

            if (xCount < BOARDLENGTH && oCount < BOARDLENGTH)
                return false;

            //Rows and columns
            for (int i = 0; i < BOARDLENGTH; i++)
            {
                for (int j = 0; j < BOARDLENGTH; j++)
                {
                    rowList.Add(Board.board[i, j]);
                    columnList.Add(Board.board[j, i]);
                }

                if(rowList.TrueForAll(o => o.Equals('X')) || columnList.TrueForAll(o => o.Equals('X')))
                {
                    isCrossWinner = true;
                    return true;
                }
                else if(rowList.TrueForAll(o => o.Equals('O')) || columnList.TrueForAll(o => o.Equals('O')))
                {
                    isCrossWinner = false;
                    return true;
                }

                rowList.Clear();
                columnList.Clear();
            }

            //Diagonals

            char toCheck = Board.board[1, 1];

            if (toCheck == 'X' || toCheck == 'O')
            {
                isCrossWinner = (toCheck == 'X') ? true : false;

                if(Board.board[0, 0] == toCheck && Board.board[2, 2] == toCheck)
                {
                    return true;
                }
                else if(Board.board[0, 2] == toCheck && Board.board[2, 0] == toCheck)
                {
                    return true;
                }
            }

            if ((xCount + oCount) == BOARDSIZE)
            {
                isDraw = true;
                return true;
            }

            return false;
        }

        public static bool WinCheck(GameState gs)
        {
            int xCount = 0;
            int oCount = 0;
            List<char> rowList = new List<char>();
            List<char> columnList = new List<char>();

            foreach (var item in gs.state)
            {
                if (item == 'O')
                {
                    oCount++;
                }
                else if (item == 'X')
                {
                    xCount++;
                }
            }

            if (xCount < BOARDLENGTH && oCount < BOARDLENGTH)
                return false;

            //Rows and columns
            for (int i = 0; i < BOARDLENGTH; i++)
            {
                for (int j = 0; j < BOARDLENGTH; j++)
                {
                    rowList.Add(gs.state[i, j]);

                    columnList.Add(gs.state[j, i]);
                }

                if (rowList.TrueForAll(o => o.Equals('X')) || columnList.TrueForAll(o => o.Equals('X')))
                {
                    return true;
                }
                else if (rowList.TrueForAll(o => o.Equals('O')) || columnList.TrueForAll(o => o.Equals('O')))
                {
                    return true;
                }

                rowList.Clear();
                columnList.Clear();
            }

            //Diagonals
            char toCheck = gs.state[1, 1];

            if (toCheck == 'X' || toCheck == 'O')
            {
                isCrossWinner = (toCheck == 'X') ? true : false;

                if (gs.state[0, 0] == toCheck && gs.state[2, 2] == toCheck)
                {
                    return true;
                }
                else if (gs.state[0, 2] == toCheck && gs.state[2, 0] == toCheck)
                {
                    return true;
                }
            }

            if ((xCount + oCount) == BOARDSIZE)
            {
                return true;
            }

            return false;
        }

        public static int EvaluateBoard(GameState gs, int depth)
        {
            int xCount = 0;
            int oCount = 0;
            List<char> rowList = new List<char>();
            List<char> columnList = new List<char>();

            foreach (var item in gs.state)
            {
                if (item == 'O')
                {
                    oCount++;
                }
                else if (item == 'X')
                {
                    xCount++;
                }
            }

            if (xCount < BOARDLENGTH && oCount < BOARDLENGTH)
                return 0;

            //Rows and columns
            for (int i = 0; i < BOARDLENGTH; i++)
            {
                for (int j = 0; j < BOARDLENGTH; j++)
                {
                    rowList.Add(gs.state[i, j]);

                    columnList.Add(gs.state[j, i]);
                }

                if (rowList.TrueForAll(o => o.Equals('X')) || columnList.TrueForAll(o => o.Equals('X')))
                {
                    return 10 - depth;
                }
                else if (rowList.TrueForAll(o => o.Equals('O')) || columnList.TrueForAll(o => o.Equals('O')))
                {
                    return -10 + depth;
                }

                rowList.Clear();
                columnList.Clear();
            }

            //Diagonals
            char toCheck = gs.state[1, 1];

            if (toCheck == 'X' || toCheck == 'O')
            {
                int score = 0;

                if (toCheck == 'X')
                    score = 10 - depth;
                else
                    score = -10 + depth;

                if (gs.state[0, 0] == toCheck && gs.state[2, 2] == toCheck)
                {
                    return score;
                }
                else if (gs.state[0, 2] == toCheck && gs.state[2, 0] == toCheck)
                {
                    return score;
                }
            }

            return 0;
        }

    }
}
