using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Rook : ChessPiece
    {
        public Rook(string color)
        {
            Color = color;
            Symbol = color == "White" ? "♖" : "♜";
        }

        public override bool IsMoveValid(int currentX, int currentY, int targetX, int targetY, ChessPiece[,] board)
        {
            // Rook can move in straight lines only (either same row or same column)
            if (currentX != targetX && currentY != targetY) return false;

            // Check for obstacles between current position and target
            if (currentX == targetX)
            {
                int step = currentY < targetY ? 1 : -1;
                for (int y = currentY + step; y != targetY; y += step)
                {
                    if (board[currentX, y] != null) return false;
                }
            }
            else
            {
                int step = currentX < targetX ? 1 : -1;
                for (int x = currentX + step; x != targetX; x += step)
                {
                    if (board[x, currentY] != null) return false;
                }
            }
            return true;
        }
    }
}
