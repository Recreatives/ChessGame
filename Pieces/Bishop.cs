using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Bishop : ChessPiece
    {
        public Bishop(string color)
        {
            Color = color;
            Symbol = color == "White" ? "♗" : "♝";
        }

        public override bool IsMoveValid(int currentX, int currentY, int targetX, int targetY, ChessPiece[,] board)
        {
            // Bishop moves diagonally
            int dx = Math.Abs(targetX - currentX);
            int dy = Math.Abs(targetY - currentY);
            if (dx != dy) return false;

            // Check for obstacles in the path
            int stepX = targetX > currentX ? 1 : -1;
            int stepY = targetY > currentY ? 1 : -1;
            for (int i = 1; i < dx; i++)
            {
                if (board[currentX + i * stepX, currentY + i * stepY] != null) return false;
            }

            return true;
        }
    }
}
