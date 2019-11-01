using ChessApp.PieceRulesets;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ChessApp
{
    public static class Logic
    {
        public static bool enPassant = false;
        public static bool castling = false;
        private static bool skipForCastleCheck = false;
        public static bool pieceToMoveFirstMove = false;
        public static bool pieceToMoveToFirstMove = false;

        private static List<Tuple<Point, Piece>> modifiedPieces = new List<Tuple<Point, Piece>>();

        public static bool CheckMove(PieceColour playerColour, Point startPoint, Point destination, bool? computerPawnPromotion = false)
        {
            if (!Board.board.ContainsKey(startPoint) ||
                !Board.board.ContainsKey(destination))
                return false;


            Piece pieceToMove = Board.board[startPoint];
            Piece pieceToMoveTo = Board.board[destination];

            skipForCastleCheck = false;

            if (((pieceToMove.type == PieceType.Rook && pieceToMoveTo.type == PieceType.King) ||
                (pieceToMove.type == PieceType.King && pieceToMoveTo.type == PieceType.Rook)) &&
                pieceToMoveTo.colour == pieceToMove.colour)
                skipForCastleCheck = true;

            if (pieceToMove.colour != playerColour)
                return false;

            if(pieceToMoveTo.colour == playerColour && !skipForCastleCheck)
            {
                return false;
            }

            //Blue goes from 1 to 8, Red goes from 8 to 1
            switch (pieceToMove.type)
            {
                case PieceType.Pawn:
                    return PawnRuleset.rules(startPoint, destination, (playerColour == PieceColour.Blue) ? true : false, computerPawnPromotion.Value);
                case PieceType.Rook:
                    return RookRuleset.rules(startPoint, destination);
                case PieceType.Knight:
                    return KnightRuleset.rules(startPoint, destination);
                case PieceType.Bishop:
                    return BishopRuleset.rules(startPoint, destination);
                case PieceType.Queen:
                    return QueenRuleset.rules(startPoint, destination);
                case PieceType.King:
                    return KingRuleset.rules(startPoint, destination);
            }

            return false;
        }

        public static void ExecuteMove(Point pieceToMove, Point positionToMoveTo)
        {
            modifiedPieces.Clear();

            modifiedPieces.Add(new Tuple<Point, Piece>(pieceToMove, Board.board[pieceToMove]));
            modifiedPieces.Add(new Tuple<Point, Piece>(positionToMoveTo, Board.board[positionToMoveTo]));

            if (enPassant)
            {
                Point enPassantTakenPiecePosition = new Point(positionToMoveTo.X, pieceToMove.Y);
                modifiedPieces.Add(new Tuple<Point, Piece>(enPassantTakenPiecePosition, Board.board[enPassantTakenPiecePosition]));
                Board.board[enPassantTakenPiecePosition] = new Piece(PieceColour.Blank, PieceType.Blank);
                enPassant = false;
            }

            if(castling)
            {
                bool highCastle = false;

                if(Board.board[pieceToMove].type == PieceType.King)
                {
                    if (pieceToMove.X - positionToMoveTo.X < 0)
                        highCastle = true;
                    else
                        highCastle = false;
                }
                else
                {
                    if (pieceToMove.X - positionToMoveTo.X < 0)
                        highCastle = false;
                    else
                        highCastle = true;
                }


                if (highCastle)
                {
                    Point newRookPosition = new Point('f', pieceToMove.Y);
                    Point newKingPosition = new Point('g', pieceToMove.Y);

                    modifiedPieces.Add(new Tuple<Point, Piece>(newRookPosition, Board.board[newRookPosition]));
                    modifiedPieces.Add(new Tuple<Point, Piece>(newKingPosition, Board.board[newKingPosition]));

                    Board.board[newRookPosition] = Board.board[new Point('h', pieceToMove.Y)];
                    Board.board[new Point('h', pieceToMove.Y)] = new Piece(PieceColour.Blank, PieceType.Blank);
                    Board.board[newKingPosition] = Board.board[new Point('e', pieceToMove.Y)];
                    Board.board[new Point('e', pieceToMove.Y)] = new Piece(PieceColour.Blank, PieceType.Blank);
                }
                else
                {
                    Point newRookPosition = new Point('d', pieceToMove.Y);
                    Point newKingPosition = new Point('c', pieceToMove.Y);

                    modifiedPieces.Add(new Tuple<Point, Piece>(newRookPosition, Board.board[newRookPosition]));
                    modifiedPieces.Add(new Tuple<Point, Piece>(newKingPosition, Board.board[newKingPosition]));

                    Board.board[newRookPosition] = Board.board[new Point('a', pieceToMove.Y)];
                    Board.board[new Point('a', pieceToMove.Y)] = new Piece(PieceColour.Blank, PieceType.Blank);
                    Board.board[newKingPosition] = Board.board[new Point('e', pieceToMove.Y)];
                    Board.board[new Point('e', pieceToMove.Y)] = new Piece(PieceColour.Blank, PieceType.Blank);
                }

                castling = false;
            }
            else
            {
                Board.board[positionToMoveTo] = Board.board[pieceToMove];
                Board.board[pieceToMove] = new Piece(PieceColour.Blank, PieceType.Blank);
            }

            pieceToMoveFirstMove = Board.board[pieceToMove].firstMove;
            pieceToMoveToFirstMove = Board.board[positionToMoveTo].firstMove;

            Board.board[pieceToMove].firstMove = false;
            Board.board[positionToMoveTo].firstMove = false;
        }

        public static bool isGameOver(PieceColour colour)
        {
            PieceColour opposingColour = colour == PieceColour.Blue ? PieceColour.Red : PieceColour.Blue;

            if (KingCheckmate(opposingColour))
            {
                Console.WriteLine($"\n{opposingColour} is in checkmate, the game is over!");
                Console.WriteLine($"\n{colour} wins!");
                return true;
            }

            return false;
        }

        public static bool KingCheck(PieceColour colour, bool? computerPawnPromotion = false)
        {
            PieceColour opposingColour = (colour == PieceColour.Blue) ? PieceColour.Red : PieceColour.Blue;

            Point kingPosition = Board.board.First(o => o.Value.colour == colour && o.Value.type == PieceType.King).Key;
            var opposingPieces = Board.board.Where(o => o.Value.colour == opposingColour);

            foreach (var pieceKV in opposingPieces)
            {
                if (CheckMove(opposingColour, pieceKV.Key, kingPosition, computerPawnPromotion.Value))
                    return true;
            }

            return false;
        }

        public static bool KingCheckmate(PieceColour colour)
        {
            var ownPieces = Board.board.Where(o => o.Value.colour == colour).ToList();

            //var possibleMoves = Board.board.Where(o => o.Value.colour != colour);
            List<Point> possibleMoves = Board.board.Keys.ToList();

            foreach (var ownPieceKV in ownPieces)
            {
                foreach (var possiblePosition in possibleMoves)
                {
                    if (CheckMove(colour, ownPieceKV.Key, possiblePosition))
                    {
                        Piece pieceToMovePiece = Board.board[ownPieceKV.Key];
                        Piece positionToMoveToPiece = Board.board[possiblePosition];

                        ExecuteMove(ownPieceKV.Key, possiblePosition);

                        if (!KingCheck(colour))
                        {
                            Reverse();

                            if (pieceToMoveFirstMove)
                                Board.board[ownPieceKV.Key].firstMove = true;

                            if (pieceToMoveToFirstMove)
                                Board.board[possiblePosition].firstMove = true;

                            return false;
                        }
                        else
                        {
                            Reverse();

                            if (pieceToMoveFirstMove)
                                Board.board[ownPieceKV.Key].firstMove = true;

                            if (pieceToMoveToFirstMove)
                                Board.board[possiblePosition].firstMove = true;
                        }
                    }
                }
            }

            return true;
        }

        public static void Reverse()
        {
            foreach (var tuple in modifiedPieces)
            {
                Board.board[tuple.Item1] = tuple.Item2;
            }
        }
    }
}
