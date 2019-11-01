using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApp
{
    public class Piece
    {
        public PieceColour colour { get; set; }
        public PieceType type { get; set; }

        public bool firstMove = true;

        public bool pawnDoubleSpace = false;

        public Piece(PieceColour newColour, PieceType newType)
        {
            colour = newColour;
            type = newType;
        }
    }
    
    public enum PieceColour
    {
        Blank = 0,
        Blue,
        Red
    }

    public enum PieceType
    {
        Blank = 0,
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }
}
