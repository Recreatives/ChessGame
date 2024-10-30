using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Pawn : ChessPiece
    {
        public Pawn(string color)
        {
            Color = color;
            Symbol = color == "White" ? "♙" : "♟";
        }

        public override bool IsMoveValid(int currentX, int currentY, int targetX, int targetY, ChessPiece?[,] board)
        {
            int direction = Color == "White" ? -1 : 1; // White pawns move up (negative Y), Black pawns move down (positive Y)
            int startRow = Color == "White" ? 6 : 1;   // Initial row for each color

            // Moving forward one square
            if (currentX == targetX && targetY == currentY + direction && board[targetX, targetY] == null)
            {
                return true;
            }

            // Moving forward two squares from the starting position
            if (currentX == targetX && currentY == startRow && targetY == currentY + 2 * direction && board[targetX, targetY] == null && board[currentX, currentY + direction] == null)
            {
                return true;
            }

            // Capturing diagonally
            if (Math.Abs(currentX - targetX) == 1 && targetY == currentY + direction && board[targetX, targetY] != null && board[targetX, targetY].Color != this.Color)
            {
                return true;
            }

            return false; // Invalid move
        }
    }
}


