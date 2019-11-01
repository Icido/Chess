using ChessApp.PieceRulesets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessApp
{
    public class MinimaxBoardLogic
    {
        public bool enPassant = false;
        public bool castling = false;
        private bool skipForCastleCheck = false;
        public bool pieceToMoveFirstMove = false;
        public bool pieceToMoveToFirstMove = false;

        private List<Tuple<Point, Piece>> modifiedPieces = new List<Tuple<Point, Piece>>();

        public bool CheckMove(PieceColour playerColour, Point startPoint, Point destination, GameState gs, bool? computerPawnPromotion = false)
        {
            if (!gs.state.ContainsKey(startPoint) ||
                !gs.state.ContainsKey(destination))
                return false;

            Piece pieceToMove = gs.state[startPoint];
            Piece pieceToMoveTo = gs.state[destination];

            skipForCastleCheck = false;

            if (((pieceToMove.type == PieceType.Rook && pieceToMoveTo.type == PieceType.King) ||
                (pieceToMove.type == PieceType.King && pieceToMoveTo.type == PieceType.Rook)) &&
                pieceToMoveTo.colour == pieceToMove.colour)
                skipForCastleCheck = true;

            if (pieceToMove.colour != playerColour)
                return false;

            if (pieceToMoveTo.colour == playerColour && !skipForCastleCheck)
            {
                return false;
            }

            //Blue goes from 1 to 8, Red goes from 8 to 1
            switch (pieceToMove.type)
            {
                case PieceType.Pawn:
                    return PawnRuleset.rules(startPoint, destination, (playerColour == PieceColour.Blue) ? true : false, gs, this, computerPawnPromotion.Value);
                case PieceType.Rook:
                    return RookRuleset.rules(startPoint, destination, gs, this);
                case PieceType.Knight:
                    return KnightRuleset.rules(startPoint, destination);
                case PieceType.Bishop:
                    return BishopRuleset.rules(startPoint, destination, gs);
                case PieceType.Queen:
                    return QueenRuleset.rules(startPoint, destination, gs);
                case PieceType.King:
                    return KingRuleset.rules(startPoint, destination, gs, this);
            }

            return false;
        }

        public void ExecuteMove(Point pieceToMove, Point positionToMoveTo, GameState gs)
        {
            modifiedPieces.Clear();

            modifiedPieces.Add(new Tuple<Point, Piece>(pieceToMove, gs.state[pieceToMove]));
            modifiedPieces.Add(new Tuple<Point, Piece>(positionToMoveTo, gs.state[positionToMoveTo]));

            if (enPassant)
            {
                Point enPassantTakenPiecePosition = new Point(positionToMoveTo.X, pieceToMove.Y);
                modifiedPieces.Add(new Tuple<Point, Piece>(enPassantTakenPiecePosition, gs.state[enPassantTakenPiecePosition]));
                gs.state[enPassantTakenPiecePosition] = new Piece(PieceColour.Blank, PieceType.Blank);
                enPassant = false;
            }

            if (castling)
            {
                bool highCastle = false;

                if (gs.state[pieceToMove].type == PieceType.King)
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

                    modifiedPieces.Add(new Tuple<Point, Piece>(newRookPosition, gs.state[newRookPosition]));
                    modifiedPieces.Add(new Tuple<Point, Piece>(newKingPosition, gs.state[newKingPosition]));

                    gs.state[newRookPosition] = gs.state[new Point('h', pieceToMove.Y)];
                    gs.state[new Point('h', pieceToMove.Y)] = new Piece(PieceColour.Blank, PieceType.Blank);
                    gs.state[newKingPosition] = gs.state[new Point('e', pieceToMove.Y)];
                    gs.state[new Point('e', pieceToMove.Y)] = new Piece(PieceColour.Blank, PieceType.Blank);
                }
                else
                {
                    Point newRookPosition = new Point('d', pieceToMove.Y);
                    Point newKingPosition = new Point('c', pieceToMove.Y);

                    modifiedPieces.Add(new Tuple<Point, Piece>(newRookPosition, gs.state[newRookPosition]));
                    modifiedPieces.Add(new Tuple<Point, Piece>(newKingPosition, gs.state[newKingPosition]));

                    gs.state[newRookPosition] = gs.state[new Point('a', pieceToMove.Y)];
                    gs.state[new Point('a', pieceToMove.Y)] = new Piece(PieceColour.Blank, PieceType.Blank);
                    gs.state[newKingPosition] = gs.state[new Point('e', pieceToMove.Y)];
                    gs.state[new Point('e', pieceToMove.Y)] = new Piece(PieceColour.Blank, PieceType.Blank);
                }

                castling = false;
            }
            else
            {
                gs.state[positionToMoveTo] = gs.state[pieceToMove];
                gs.state[pieceToMove] = new Piece(PieceColour.Blank, PieceType.Blank);
            }

            pieceToMoveFirstMove = gs.state[pieceToMove].firstMove;
            pieceToMoveToFirstMove = gs.state[positionToMoveTo].firstMove;

            gs.state[pieceToMove].firstMove = false;
            gs.state[positionToMoveTo].firstMove = false;
        }

        public bool isGameOver(PieceColour colour, GameState gs)
        {
            PieceColour opposingColour = colour == PieceColour.Blue ? PieceColour.Red : PieceColour.Blue;

            if (KingCheckmate(opposingColour, gs))
            {
                Console.WriteLine($"\n{opposingColour} is in checkmate, the game is over!");
                Console.WriteLine($"\n{colour} wins!");
                return true;
            }

            return false;
        }

        public bool KingCheck(PieceColour colour, GameState gs, bool? computerPawnPromotion = false)
        {
            PieceColour opposingColour = (colour == PieceColour.Blue) ? PieceColour.Red : PieceColour.Blue;

            Point kingPosition = gs.state.First(o => o.Value.colour == colour && o.Value.type == PieceType.King).Key;
            var opposingPieces = gs.state.Where(o => o.Value.colour == opposingColour);

            foreach (var pieceKV in opposingPieces)
            {
                if (CheckMove(opposingColour, pieceKV.Key, kingPosition, gs, computerPawnPromotion.Value))
                    return true;
            }

            return false;
        }

        public bool KingCheckmate(PieceColour colour, GameState gs)
        {
            var ownPieces = gs.state.Where(o => o.Value.colour == colour).ToList();

            //var possibleMoves = gs.state.Where(o => o.Value.colour != colour);
            List<Point> possibleMoves = gs.state.Keys.ToList();

            foreach (var ownPieceKV in ownPieces)
            {
                foreach (var possiblePosition in possibleMoves)
                {
                    if (CheckMove(colour, ownPieceKV.Key, possiblePosition, gs))
                    {
                        Piece pieceToMovePiece = gs.state[ownPieceKV.Key];
                        Piece positionToMoveToPiece = gs.state[possiblePosition];

                        ExecuteMove(ownPieceKV.Key, possiblePosition, gs);

                        if (!KingCheck(colour, gs, true))
                        {
                            Reverse(gs);

                            if (pieceToMoveFirstMove)
                                gs.state[ownPieceKV.Key].firstMove = true;

                            if (pieceToMoveToFirstMove)
                                gs.state[possiblePosition].firstMove = true;

                            return false;
                        }
                        else
                        {
                            Reverse(gs);

                            if (pieceToMoveFirstMove)
                                gs.state[ownPieceKV.Key].firstMove = true;

                            if (pieceToMoveToFirstMove)
                                gs.state[possiblePosition].firstMove = true;
                        }
                    }
                }
            }

            return true;
        }

        public int BoardCheck(PieceColour colour, GameState gs)
        {
            PieceColour otherColour = (colour == PieceColour.Blue) ? PieceColour.Red : PieceColour.Blue;

            if (KingCheckmate(otherColour, gs))
                return 10;
            else if (KingCheckmate(colour, gs))
                return -10;

            if (KingCheck(otherColour, gs, true))
                return 1;
            else if (KingCheck(colour, gs, true))
                return -1;

            return 0;
        }

        public void Reverse(GameState gs)
        {
            foreach (var tuple in modifiedPieces)
            {
                gs.state[tuple.Item1] = tuple.Item2;
            }
        }

    }
}
