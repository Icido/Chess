using System;
using System.Collections.Generic;
using System.Text;

namespace NoughtsAndCrossesApp
{
    public static class Computer
    {
        public static int minimax()
        {
            StateNode sn = new StateNode();
            return TupleToMove(sn.miniMax(Board.board, false));
        }

        public static int TupleToMove(Tuple<int, int> action)
        {
            switch (action)
            {
                case var tuple when tuple.Item1 == 0 && tuple.Item2 == 0:
                    return 1;
                case var tuple when tuple.Item1 == 0 && tuple.Item2 == 1:
                    return 2;
                case var tuple when tuple.Item1 == 0 && tuple.Item2 == 2:
                    return 3;
                case var tuple when tuple.Item1 == 1 && tuple.Item2 == 0:
                    return 4;
                case var tuple when tuple.Item1 == 1 && tuple.Item2 == 1:
                    return 5;
                case var tuple when tuple.Item1 == 1 && tuple.Item2 == 2:
                    return 6;
                case var tuple when tuple.Item1 == 2 && tuple.Item2 == 0:
                    return 7;
                case var tuple when tuple.Item1 == 2 && tuple.Item2 == 1:
                    return 8;
                case var tuple when tuple.Item1 == 2 && tuple.Item2 == 2:
                    return 9;
                default:
                    break;
            }

            return -1;
        }
    }
}
