using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApp
{
    public class Computer
    {
        private Dictionary<Point, Piece> gameState = new Dictionary<Point, Piece>();

        public void computerMove(PieceColour colour)
        {
            //TODO: Figure out why computer modifies the entire board when checking states, then finally moves a piece that's not theirs.

            gameState.Clear();
            gameState = Board.board;
            StateNode sn = new StateNode();

            Tuple<char, int, char, int> move = sn.miniMax(gameState, colour);
            

            Point pieceToMove = new Point(move.Item1, move.Item2);
            Point positionToMoveTo = new Point(move.Item3, move.Item4);

            Logic.ExecuteMove(pieceToMove, positionToMoveTo);

            Console.WriteLine($"Moved {Board.board[positionToMoveTo].type} from {pieceToMove.X}{pieceToMove.Y} to {positionToMoveTo.X}{positionToMoveTo.Y}");


        }
    }
}
