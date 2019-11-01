using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApp.PieceRulesets
{
    public static class KingRuleset
    {
        public static bool rules(Point piece, Point destination)
        {
            int xDistance = Math.Abs(piece.X - destination.X);
            int yDistance = Math.Abs(piece.Y - destination.Y);

            if (Board.board[piece].firstMove == true)
            {
                Point tempRookPoint = piece;

                if (piece.X - destination.X > 0)
                    tempRookPoint = new Point('a', piece.Y);
                else if (piece.X - destination.X < 0)
                    tempRookPoint = new Point('h', piece.Y);
                    

                if (destination.X == tempRookPoint.X && destination.Y == tempRookPoint.Y)
                {
                    Piece tempRookPiece = Board.board[tempRookPoint];

                    if (tempRookPiece.type == PieceType.Rook && tempRookPiece.colour == Board.board[piece].colour &&
                        IterationCheck.isNoPieceBetweenLinear(piece, tempRookPoint) && tempRookPiece.firstMove == true)
                    {
                        Logic.castling = true;
                        return true;
                    }
                }
            }

            if (xDistance > 1 || yDistance > 1)
                return false;

            return true;
        }

        public static bool rules(Point piece, Point destination, GameState gs, MinimaxBoardLogic logic)
        {
            int xDistance = Math.Abs(piece.X - destination.X);
            int yDistance = Math.Abs(piece.Y - destination.Y);

            if (gs.state[piece].firstMove == true)
            {
                Point tempRookPoint = piece;

                if (piece.X - destination.X > 0)
                    tempRookPoint = new Point('a', piece.Y);
                else if (piece.X - destination.X < 0)
                    tempRookPoint = new Point('h', piece.Y);
                    

                if (destination.X == tempRookPoint.X && destination.Y == tempRookPoint.Y)
                {
                    Piece tempRookPiece = gs.state[tempRookPoint];

                    if (tempRookPiece.type == PieceType.Rook && tempRookPiece.colour == gs.state[piece].colour &&
                        IterationCheck.isNoPieceBetweenLinear(piece, tempRookPoint, gs) && tempRookPiece.firstMove == true)
                    {
                        logic.castling = true;
                        return true;
                    }
                }
            }

            if (xDistance > 1 || yDistance > 1)
                return false;

            return true;
        }
    }
}
