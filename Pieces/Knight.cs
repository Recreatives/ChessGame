using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Knight : ChessPiece
    {
        public Knight(string color)
        {
            Color = color;
            Symbol = color == "White" ? "♘" : "♞";
        }

        public override bool IsMoveValid(int currentX, int currentY, int targetX, int targetY, ChessPiece[,] board)
        {
            // Knights move in an "L" shape: two squares in one direction and one square perpendicular
            int dx = Math.Abs(targetX - currentX);
            int dy = Math.Abs(targetY - currentY);
            return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
        }
    }
}
