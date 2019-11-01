using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApp
{
    public class GameState
    {
        public Dictionary<Point, Piece> state;
        public PieceColour currentPlayer;

        public GameState()
        {
            state = new Dictionary<Point, Piece>();
        }

        public bool Equals(GameState gs)
        {
            foreach (var item in gs.state)
            {
                if (state.ContainsKey(item.Key))
                {
                    if (state[item.Key] != item.Value)
                        return false;
                }
            }

            return true;
        }

        public GameState Copy()
        {
            var copy = new GameState();
            copy.state = new Dictionary<Point, Piece>();

            foreach (var item in state)
            {
                copy.state.Add(item.Key, item.Value);
            }

            return copy;
        }
    }
}
