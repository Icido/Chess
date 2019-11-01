using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApp.PieceRulesets
{
    public static class PawnRuleset
    {
        public static bool rules(Point piece, Point destination, bool from1to8, bool? computerPawnPromotion = false)
        {
            int xDistance = Math.Abs(piece.X - destination.X);
            int yDistance = piece.Y - destination.Y;
            bool pawnsFirstMove = Board.board[piece].firstMove;

            if (from1to8)
            {
                if (yDistance != -1 && yDistance != -2)
                    return false;
                else if (yDistance == -2 && !pawnsFirstMove)
                    return false;
                else if (!IterationCheck.isNoPieceBetweenLinear(piece, destination))
                    return false;
            }
            else
            {
                if (yDistance != 1 && yDistance != 2)
                    return false;
                else if (yDistance == 2 && !pawnsFirstMove)
                    return false;
                else if (!IterationCheck.isNoPieceBetweenLinear(piece, destination))
                    return false;
            }

            if (xDistance != 0 && xDistance != 1)
                return false;
            else if (xDistance == 0 && Board.board[destination].type != PieceType.Blank)
                return false;
            else if (xDistance == 1 && Board.board[destination].type == PieceType.Blank &&
                Board.board[new Point(destination.X, piece.Y)].type != PieceType.Pawn)
                return false;
            else if (xDistance == 1 && Board.board[destination].type == PieceType.Blank &&
                Board.board[new Point(destination.X, piece.Y)].type == PieceType.Pawn &&
                Board.board[new Point(destination.X, piece.Y)].pawnDoubleSpace == false) // En passant
                return false;

            if (Math.Abs(yDistance) == 2)
                Board.board[piece].pawnDoubleSpace = true;

            if (xDistance == 1 && Board.board[destination].type == PieceType.Blank &&
                Board.board[new Point(destination.X, piece.Y)].type == PieceType.Pawn &&
                Board.board[new Point(destination.X, piece.Y)].colour != Board.board[piece].colour &&
                Board.board[new Point(destination.X, piece.Y)].pawnDoubleSpace == true)
                Logic.enPassant = true;

            if (((from1to8 && (destination.Y == 8) )||
                (!from1to8 && (destination.Y == 1))))
            {
                char inputChar;
                bool promotionCompleted = false;

                while (!promotionCompleted)
                {
                    if (!computerPawnPromotion.Value)
                    {
                        Console.WriteLine("Pawn promotion! Please select a new peice:");
                        inputChar = char.ToLower(Console.ReadKey().KeyChar);
                    }
                    else
                        inputChar = 'q';
                    
                    switch (inputChar)
                    {
                        case 'q':
                            Board.board[piece].type = PieceType.Queen;
                            promotionCompleted = true;
                            break;
                        case 'r':
                            Board.board[piece].type = PieceType.Rook;
                            promotionCompleted = true;
                            break;
                        case 'n':
                            Board.board[piece].type = PieceType.Knight;
                            promotionCompleted = true;
                            break;
                        case 'b':
                            Board.board[piece].type = PieceType.Bishop;
                            promotionCompleted = true;
                            break;
                        default:
                            Console.WriteLine("Incorrect entry - please enter what new piece you would like (q, r, n, b)");
                            break;
                    }
                }
            }

            return true;
        }

        public static bool rules(Point piece, Point destination, bool from1to8, GameState gs, MinimaxBoardLogic logic, bool? computerPawnPromotion = false)
        {
            int xDistance = Math.Abs(piece.X - destination.X);
            int yDistance = piece.Y - destination.Y;
            bool pawnsFirstMove = gs.state[piece].firstMove;

            if (from1to8)
            {
                if (yDistance != -1 && yDistance != -2)
                    return false;
                else if (yDistance == -2 && !pawnsFirstMove)
                    return false;
                else if (!IterationCheck.isNoPieceBetweenLinear(piece, destination, gs))
                    return false;
            }
            else
            {
                if (yDistance != 1 && yDistance != 2)
                    return false;
                else if (yDistance == 2 && !pawnsFirstMove)
                    return false;
                else if (!IterationCheck.isNoPieceBetweenLinear(piece, destination, gs))
                    return false;
            }

            if (xDistance != 0 && xDistance != 1)
                return false;
            else if (xDistance == 0 && gs.state[destination].type != PieceType.Blank)
                return false;
            else if (xDistance == 1 && gs.state[destination].type == PieceType.Blank &&
                gs.state[new Point(destination.X, piece.Y)].type != PieceType.Pawn)
                return false;
            else if (xDistance == 1 && gs.state[destination].type == PieceType.Blank &&
                gs.state[new Point(destination.X, piece.Y)].type == PieceType.Pawn &&
                gs.state[new Point(destination.X, piece.Y)].pawnDoubleSpace == false) // En passant
                return false;

            if (Math.Abs(yDistance) == 2)
                gs.state[piece].pawnDoubleSpace = true;

            if (xDistance == 1 && gs.state[destination].type == PieceType.Blank &&
                gs.state[new Point(destination.X, piece.Y)].type == PieceType.Pawn &&
                gs.state[new Point(destination.X, piece.Y)].colour != gs.state[piece].colour &&
                gs.state[new Point(destination.X, piece.Y)].pawnDoubleSpace == true)
                logic.enPassant = true;

            if (((from1to8 && (destination.Y == 8) )||
                (!from1to8 && (destination.Y == 1))))
            {

                char inputChar;
                bool promotionCompleted = false;

                while (!promotionCompleted)
                {
                    if (!computerPawnPromotion.Value)
                    {
                        Console.WriteLine("Pawn promotion! Please select a new peice:");
                        inputChar = char.ToLower(Console.ReadKey().KeyChar);
                    }
                    else
                        inputChar = 'q';
                    
                    switch (inputChar)
                    {
                        case 'q':
                            gs.state[piece].type = PieceType.Queen;
                            promotionCompleted = true;
                            break;
                        case 'r':
                            gs.state[piece].type = PieceType.Rook;
                            promotionCompleted = true;
                            break;
                        case 'n':
                            gs.state[piece].type = PieceType.Knight;
                            promotionCompleted = true;
                            break;
                        case 'b':
                            gs.state[piece].type = PieceType.Bishop;
                            promotionCompleted = true;
                            break;
                        default:
                            Console.WriteLine("Incorrect entry - please enter what new piece you would like (q, r, n, b)");
                            break;
                    }
                }
            }

            return true;
        }
    }
}
