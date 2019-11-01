using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApp.PieceRulesets
{
    public static class KnightRuleset
    {
        public static bool rules(Point piece, Point destination)
        {
            int xDistance = Math.Abs(piece.X - destination.X);
            int yDistance = Math.Abs(piece.Y - destination.Y);

            if (xDistance == yDistance)
                return false;
            else if (xDistance == 0 || yDistance == 0)
                return false;
            else if (xDistance > yDistance)
            {
                if (xDistance != 2 || yDistance != 1)
                    return false;
            }
            else if (yDistance > xDistance)
            {
                if (xDistance != 1 || yDistance != 2)
                    return false;
            }

            return true;
        }
    }
}
