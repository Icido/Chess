using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApp.PieceRulesets
{
    public static class RookRuleset
    {
        public static bool rules(Point piece, Point destination)
        {
            int xDistance = Math.Abs(piece.X - destination.X);
            int yDistance = Math.Abs(piece.Y - destination.Y);

            if (Board.board[piece].firstMove == true)
            {
                Point tempKingPoint = new Point('e', piece.Y);

                if (destination.X == tempKingPoint.X && destination.Y == tempKingPoint.Y)
                {
                    Piece tempKingPiece = Board.board[tempKingPoint];

                    if (tempKingPiece.type == PieceType.King && tempKingPiece.colour == Board.board[piece].colour &&
                        IterationCheck.isNoPieceBetweenLinear(piece, tempKingPoint) && tempKingPiece.firstMove == true)
                    {
                        Logic.castling = true;
                        return true;
                    }
                }
            }

            if (xDistance != 0 && yDistance != 0)
                return false;
            else if (xDistance > 0 && yDistance > 0)
                return false;
            else if (xDistance > 0 || yDistance > 0)
                if (!IterationCheck.isNoPieceBetweenLinear(piece, destination))
                    return false;

            return true;
        }

        public static bool rules(Point piece, Point destination, GameState gs, MinimaxBoardLogic logic)
        {
            int xDistance = Math.Abs(piece.X - destination.X);
            int yDistance = Math.Abs(piece.Y - destination.Y);

            if (gs.state[piece].firstMove == true)
            {
                Point tempKingPoint = new Point('e', piece.Y);

                if (destination.X == tempKingPoint.X && destination.Y == tempKingPoint.Y)
                {
                    Piece tempKingPiece = gs.state[tempKingPoint];

                    if (tempKingPiece.type == PieceType.King && tempKingPiece.colour == gs.state[piece].colour &&
                        IterationCheck.isNoPieceBetweenLinear(piece, tempKingPoint, gs) && tempKingPiece.firstMove == true)
                    {
                        logic.castling = true;
                        return true;
                    }
                }
            }

            if (xDistance != 0 && yDistance != 0)
                return false;
            else if (xDistance > 0 && yDistance > 0)
                return false;
            else if (xDistance > 0 || yDistance > 0)
                if (!IterationCheck.isNoPieceBetweenLinear(piece, destination, gs))
                    return false;

            return true;
        }
    }
}
