using System;
using System.Collections.Generic;
using System.Text;

namespace NoughtsAndCrossesApp
{
    public class GameState
    {
        public char[,] state;

        public GameState()
        {
            state = new char[3, 3];
        }

        public bool Equals(GameState gs)
        {
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    if(state[i, j] != gs.state[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public GameState Copy()
        {
            var copy = new GameState();
            copy.state = new char[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    copy.state[i, j] = state[i, j];
                }
            }

            return copy;
        }
    }
}
