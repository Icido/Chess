using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApp.PieceRulesets
{
    public static class IterationCheck
    {
        public static bool isNoPieceBetweenLinear(Point startPoint, Point endPoint)
        {
            int xDistance = startPoint.X - endPoint.X;
            int yDistance = startPoint.Y - endPoint.Y;

            if(Math.Abs(xDistance) > Math.Abs(yDistance))
            {
                if(xDistance > 1)
                {
                    xDistance -= 1;
                    while(xDistance > 0)
                    {
                        if (Board.board[new Point((char)(endPoint.X + xDistance), startPoint.Y)].type != PieceType.Blank)
                            return false;

                        xDistance -= 1;
                    }
                }
                else
                {
                    xDistance += 1;
                    while(xDistance < 0)
                    {
                        if (Board.board[new Point((char)(endPoint.X + xDistance), startPoint.Y)].type != PieceType.Blank)
                            return false;

                        xDistance += 1;
                    }
                }
            }
            else
            {
                if (yDistance > 1)
                {
                    yDistance -= 1;
                    while (yDistance > 0)
                    {
                        if (Board.board[new Point(startPoint.X, endPoint.Y + yDistance)].type != PieceType.Blank)
                            return false;

                        yDistance -= 1;
                    }
                }
                else
                {
                    yDistance += 1;
                    while (yDistance < 0)
                    {
                        if (Board.board[new Point(startPoint.X, endPoint.Y + yDistance)].type != PieceType.Blank)
                            return false;

                        yDistance += 1;
                    }
                }
            }

            return true;
        }

        public static bool isNoPieceBetweenDiagonal(Point startPoint, Point endPoint)
        {
            int xDistance = startPoint.X - endPoint.X;
            int yDistance = startPoint.Y - endPoint.Y;

            if (xDistance > 1 && yDistance > 1)
            {
                xDistance -= 1;
                yDistance -= 1;
                while (xDistance > 0 && yDistance > 0)
                {
                    if (Board.board[new Point((char)(endPoint.X + xDistance), endPoint.Y + yDistance)].type != PieceType.Blank)
                        return false;

                    xDistance -= 1;
                    yDistance -= 1;
                }
            }
            else if (xDistance > 1 && yDistance < -1)
            {
                xDistance -= 1;
                yDistance += 1;
                while (xDistance > 0 && yDistance < 0)
                {
                    if (Board.board[new Point((char)(endPoint.X + xDistance), endPoint.Y + yDistance)].type != PieceType.Blank)
                        return false;

                    xDistance -= 1;
                    yDistance += 1;
                }
            }
            else if (xDistance < -1 && yDistance > 1)
            {
                xDistance += 1;
                yDistance -= 1;
                while (xDistance < 0 && yDistance > 0)
                {
                    if (Board.board[new Point((char)(endPoint.X + xDistance), endPoint.Y + yDistance)].type != PieceType.Blank)
                        return false;

                    xDistance += 1;
                    yDistance -= 1;
                }
            }
            else if (xDistance < -1 && yDistance < -1)
            {
                xDistance += 1;
                yDistance += 1;
                while (xDistance < 0 && yDistance < 0)
                {
                    if (Board.board[new Point((char)(endPoint.X + xDistance), endPoint.Y + yDistance)].type != PieceType.Blank)
                        return false;

                    xDistance += 1;
                    yDistance += 1;
                }
            }

            return true;
        }

        public static bool isNoPieceBetweenLinear(Point startPoint, Point endPoint, GameState gs)
        {
            int xDistance = startPoint.X - endPoint.X;
            int yDistance = startPoint.Y - endPoint.Y;

            if(Math.Abs(xDistance) > Math.Abs(yDistance))
            {
                if(xDistance > 1)
                {
                    xDistance -= 1;
                    while(xDistance > 0)
                    {
                        if (gs.state[new Point((char)(endPoint.X + xDistance), startPoint.Y)].type != PieceType.Blank)
                            return false;

                        xDistance -= 1;
                    }
                }
                else
                {
                    xDistance += 1;
                    while(xDistance < 0)
                    {
                        if (gs.state[new Point((char)(endPoint.X + xDistance), startPoint.Y)].type != PieceType.Blank)
                            return false;

                        xDistance += 1;
                    }
                }
            }
            else
            {
                if (yDistance > 1)
                {
                    yDistance -= 1;
                    while (yDistance > 0)
                    {
                        if (gs.state[new Point(startPoint.X, endPoint.Y + yDistance)].type != PieceType.Blank)
                            return false;

                        yDistance -= 1;
                    }
                }
                else
                {
                    yDistance += 1;
                    while (yDistance < 0)
                    {
                        if (gs.state[new Point(startPoint.X, endPoint.Y + yDistance)].type != PieceType.Blank)
                            return false;

                        yDistance += 1;
                    }
                }
            }

            return true;
        }

        public static bool isNoPieceBetweenDiagonal(Point startPoint, Point endPoint, GameState gs)
        {
            int xDistance = startPoint.X - endPoint.X;
            int yDistance = startPoint.Y - endPoint.Y;

            if (xDistance > 1 && yDistance > 1)
            {
                xDistance -= 1;
                yDistance -= 1;
                while(xDistance > 0 && yDistance > 0)
                {
                    if (gs.state[new Point((char)(endPoint.X + xDistance), endPoint.Y + yDistance)].type != PieceType.Blank)
                        return false;

                    xDistance -= 1;
                    yDistance -= 1;
                }
            }
            else if (xDistance > 1 && yDistance < -1)
            {
                xDistance -= 1;
                yDistance += 1;
                while (xDistance > 0 && yDistance < 0)
                {
                    if (gs.state[new Point((char)(endPoint.X + xDistance), endPoint.Y + yDistance)].type != PieceType.Blank)
                        return false;

                    xDistance -= 1;
                    yDistance += 1;
                }
            }
            else if (xDistance < -1 && yDistance > 1)
            {
                xDistance += 1;
                yDistance -= 1;
                while (xDistance < 0 && yDistance > 0)
                {
                    if (gs.state[new Point((char)(endPoint.X + xDistance), endPoint.Y + yDistance)].type != PieceType.Blank)
                        return false;

                    xDistance += 1;
                    yDistance -= 1;
                }
            }
            else if (xDistance < -1 && yDistance < -1)
            {
                xDistance += 1;
                yDistance += 1;
                while (xDistance < 0 && yDistance < 0)
                {
                    if (gs.state[new Point((char)(endPoint.X + xDistance), endPoint.Y + yDistance)].type != PieceType.Blank)
                        return false;

                    xDistance += 1;
                    yDistance += 1;
                }
            }

            return true;
        }
    }
}
