using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public abstract class ChessPiece
    {
        public string Color { get; set; }  // "White" or "Black"
        public string Symbol { get; set; } // Unicode symbol for the piece

        // Abstract method to be implemented by derived classes
        public abstract bool IsMoveValid(int currentX, int currentY, int targetX, int targetY, ChessPiece[,] board);
    }
}
