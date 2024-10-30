using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Queen : ChessPiece
    {
        public Queen(string color)
        {
            Color = color;
            Symbol = color == "White" ? "♕" : "♛";
        }

        public override bool IsMoveValid(int currentX, int currentY, int targetX, int targetY, ChessPiece[,] board)
        {
            // Queen can move like a rook or a bishop
            Rook rook = new Rook(Color);
            Bishop bishop = new Bishop(Color);
            return rook.IsMoveValid(currentX, currentY, targetX, targetY, board) ||
                   bishop.IsMoveValid(currentX, currentY, targetX, targetY, board);
        }
    }
}
