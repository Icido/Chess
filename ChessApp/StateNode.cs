using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessApp
{
    public class StateNode
    {
        public GameState gs;
        public StateNode parent;
        public List<StateNode> children;
        public int minimaxValue;
        public PieceColour player;
        public Tuple<char, int, char, int> action = new Tuple<char, int, char, int>('z', -1, 'z', -1);
        private MinimaxBoardLogic logic;

        public StateNode()
        {
            logic = new MinimaxBoardLogic();
            gs = new GameState();
            children = new List<StateNode>();
        }

        public int maxDepth;

        public Tuple<char, int, char, int> miniMax(Dictionary<Point, Piece> state, PieceColour startingPlayer)
        {
            gs.state = state;
            gs.currentPlayer = startingPlayer;
            GenerateStates(this, 2, startingPlayer);
            //minimaxValue = GenerateStates(this, 0, isMaximizingPlayer, int.MinValue, int.MaxValue);
            var listOfMoves = children.FindAll(o => o.minimaxValue == minimaxValue);

            return action;
        }

        private void GenerateStates(StateNode root, int depth, PieceColour startingPlayer)
        {
            var ownPieces = root.gs.state.Where(o => o.Value.colour == startingPlayer).ToList();

            List<Point> possibleMoves = root.gs.state.Keys.ToList();

            foreach (var ownPieceKV in ownPieces)
            {
                foreach (var possiblePosition in possibleMoves)
                {
                    if (depth > 0 && logic.CheckMove(startingPlayer, ownPieceKV.Key, possiblePosition, root.gs))
                    {
                        if(IsTerminalState(root.gs, startingPlayer) == false)
                        {
                            logic.ExecuteMove(ownPieceKV.Key, possiblePosition, root.gs);

                            if (!logic.KingCheck(startingPlayer, root.gs))
                            {
                                var copiedGameState = root.gs.Copy();
                                copiedGameState.currentPlayer = (startingPlayer == PieceColour.Blue) ? PieceColour.Red : PieceColour.Blue;

                                var newNode = new StateNode();
                                newNode.gs = copiedGameState;
                                newNode.parent = root;
                                root.children.Add(newNode);

                                newNode.action = Tuple.Create(ownPieceKV.Key.X, ownPieceKV.Key.Y, possiblePosition.X, possiblePosition.Y);

                                GenerateStates(newNode, depth - 1, copiedGameState.currentPlayer);

                                logic.Reverse(root.gs);
                            }
                        }
                    }
                }
            }

            if (root.children.Count == 0)
            {
                root.minimaxValue = Utility(root, root.player);
            }
            else
            {
                if (root.player == PieceColour.Blue)
                {
                    var max = int.MinValue;

                    foreach (var child in root.children)
                    {
                        if (child.minimaxValue > max)
                        {
                            max = child.minimaxValue;
                            root.action = child.action;
                        }
                    }

                    root.minimaxValue = max;
                }
                else
                {
                    var min = int.MaxValue;

                    foreach (var child in root.children)
                    {
                        if (child.minimaxValue < min)
                        {
                            min = child.minimaxValue;
                            root.action = child.action;
                        }
                    }

                    root.minimaxValue = min;
                }
            }
        }

        private bool IsTerminalState(GameState gs, PieceColour playerColour)
        {
            return logic.KingCheckmate(playerColour, gs);
        }

        private int Utility(StateNode root, PieceColour playerColour)
        {
            return logic.BoardCheck(player, root.gs);
        }
    }
}
