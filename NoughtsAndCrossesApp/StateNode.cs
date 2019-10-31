using System;
using System.Collections.Generic;
using System.Text;

namespace NoughtsAndCrossesApp
{
    public class StateNode
    {
        public GameState gs;
        public StateNode parent;
        public List<StateNode> children;
        public int minimaxValue = 0;
        public int currentDepth = 0;
        public int maxDepth = 9;
        public char player = 'X'; //MAXIMIZING
        public char computer = 'O'; //MINIMIZING
        public Tuple<int, int> action = new Tuple<int, int>(-1, -1);

        public StateNode()
        {
            gs = new GameState();
            children = new List<StateNode>();
        }
        
        public Tuple<int, int> miniMax(char[,] state, bool isMaximizingPlayer)
        {
            gs.state = state;
            minimaxValue = GenerateStates(this, 0, isMaximizingPlayer, int.MinValue, int.MaxValue);

            var listOfMoves = children.FindAll(o => o.minimaxValue == minimaxValue);
            if (listOfMoves.Count > 1)
            {
                Random random = new Random();
                return listOfMoves[random.Next(listOfMoves.Count)].action;
            }
            else if(listOfMoves.Count == 1)
            {
                return listOfMoves[0].action;
            }
            else
            {
                Console.WriteLine("Warning - no moves available for miniMax!");
                return new Tuple<int, int>(-1, -1);
            }
        }

        private int GenerateStates(StateNode root, int depth, bool isMaximizingPlayer, int alpha, int beta)
        {
            root.currentDepth = depth;
            if (Logic.WinCheck(root.gs) || depth >= maxDepth)
                return Logic.EvaluateBoard(root.gs, depth);

            if (isMaximizingPlayer)
            {
                root.minimaxValue = int.MinValue;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (root.gs.state[i, j] != 'X' && root.gs.state[i, j] != 'O')
                        {
                            var copiedGameState = root.gs.Copy();
                            var position = copiedGameState.state[i, j];
                            copiedGameState.state[i, j] = player;

                            var newNode = new StateNode();
                            newNode.gs = copiedGameState;
                            newNode.parent = root;
                            root.children.Add(newNode);

                            newNode.action = Tuple.Create(i, j);
                            newNode.minimaxValue = GenerateStates(newNode, depth + 1, !isMaximizingPlayer, alpha, beta);
                            root.minimaxValue = Math.Max(root.minimaxValue, newNode.minimaxValue);
                            alpha = Math.Max(alpha, root.minimaxValue);

                            if (beta <= alpha)
                                break;
                        }
                    }
                }

                return root.minimaxValue;
            }
            else
            {
                root.minimaxValue = int.MaxValue;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (root.gs.state[i, j] != 'X' && root.gs.state[i, j] != 'O')
                        {
                            var copiedGameState = root.gs.Copy();
                            var position = copiedGameState.state[i, j];
                            copiedGameState.state[i, j] = computer;

                            var newNode = new StateNode();
                            newNode.gs = copiedGameState;
                            newNode.parent = root;
                            root.children.Add(newNode);

                            newNode.action = Tuple.Create(i, j);
                            newNode.minimaxValue = GenerateStates(newNode, depth + 1, !isMaximizingPlayer, alpha, beta);
                            root.minimaxValue = Math.Min(root.minimaxValue, newNode.minimaxValue);
                            beta = Math.Min(beta, root.minimaxValue);

                            if (beta <= alpha)
                                break;
                        }
                    }
                }

                return root.minimaxValue;
            }
        }
    }
}
