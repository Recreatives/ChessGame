using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class King : ChessPiece
    {
        public King(string color)
        {
            Color = color;
            Symbol = color == "White" ? "♔" : "♚";
        }

        public override bool IsMoveValid(int currentX, int currentY, int targetX, int targetY, ChessPiece[,] board)
        {
            int dx = Math.Abs(targetX - currentX);
            int dy = Math.Abs(targetY - currentY);
            return dx <= 1 && dy <= 1;
        }
    }
}

