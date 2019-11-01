using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApp.PieceRulesets
{
    public static class BishopRuleset
    {
        public static bool rules(Point piece, Point destination)
        {
            int xDistance = Math.Abs(piece.X - destination.X);
            int yDistance = Math.Abs(piece.Y - destination.Y);

            if (xDistance == 0 || yDistance == 0)
                return false;
            else if (xDistance != yDistance)
                return false;
            else if (xDistance > 0)
                return IterationCheck.isNoPieceBetweenDiagonal(piece, destination);

            return false;
        }

        public static bool rules(Point piece, Point destination, GameState gs)
        {
            int xDistance = Math.Abs(piece.X - destination.X);
            int yDistance = Math.Abs(piece.Y - destination.Y);

            if (xDistance == 0 || yDistance == 0)
                return false;
            else if (xDistance != yDistance)
                return false;
            else if (xDistance > 0)
                return IterationCheck.isNoPieceBetweenDiagonal(piece, destination, gs);

            return false;
        }
    }
}
