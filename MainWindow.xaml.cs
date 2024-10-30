using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChessGame.Pieces;

namespace ChessGame
{
    public partial class MainWindow : Window
    {
        private Button[,] _squares = new Button[8, 8];
        private ChessPiece[,] _board = new ChessPiece[8, 8];
        private Button? _selectedSquare = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeChessBoard();
            PlacePieces();
        }

        private bool isWhiteTurn = true; // Start with White's turn

        private string GetCurrentPlayerColor()
        {
            return isWhiteTurn ? "White" : "Black";
        }

        private void SwitchTurns()
        {
            isWhiteTurn = !isWhiteTurn;
        }


        // Initialize the chessboard with alternating colors
        private void InitializeChessBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var square = new Button();
                    square.Click += OnSquareClicked;
                    square.Tag = new { Row = row, Column = col };

                    if ((row + col) % 2 == 0)
                        square.Background = Brushes.White;
                    else
                        square.Background = Brushes.Gray;

                    square.FontSize = 32;
                    square.FontWeight = FontWeights.Bold;

                    Grid.SetRow(square, row);
                    Grid.SetColumn(square, col);
                    ChessBoard.Children.Add(square);
                    _squares[row, col] = square;
                }
            }
        }

        private bool IsKingInCheck(string playerColor)
        {
            // Find the king's position
            int kingRow = -1, kingCol = -1;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (_board[row, col] is King king && king.Color == playerColor)
                    {
                        kingRow = row;
                        kingCol = col;
                        break;
                    }
                }
            }

            // Check if any opponent piece can move to the king's position
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece? piece = _board[row, col];
                    if (piece != null && piece.Color != playerColor && piece.IsMoveValid(row, col, kingRow, kingCol, _board))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        // Event handler for clicking on a chessboard square
        private void OnSquareClicked(object sender, RoutedEventArgs e)
        {
            // Ensure the sender is a Button and it has a valid Tag
            if (sender is Button square && square.Tag is not null)
            {
                var position = (dynamic)square.Tag;
                int row = position.Row;
                int col = position.Column;

                // Step 1: Handle the case where no piece is currently selected
                if (_selectedSquare == null)
                {
                    // If there's a piece on the selected square and it belongs to the current player
                    if (_board[row, col] != null && _board[row, col].Color == GetCurrentPlayerColor())
                    {
                        // Select the piece and visually highlight the square
                        _selectedSquare = square;
                        square.BorderBrush = Brushes.Red;
                        MessageBox.Show($"Piece selected at {row}, {col}"); // Debug output
                    }
                }
                else
                {
                    // Step 2: Handle the case where a piece is already selected
                    var selectedPosition = (dynamic)_selectedSquare.Tag;
                    int selectedRow = selectedPosition.Row;
                    int selectedCol = selectedPosition.Column;

                    ChessPiece selectedPiece = _board[selectedRow, selectedCol];
                    if (selectedPiece != null)
                    {
                        // Validate if the move is allowed for the selected piece
                        if (selectedPiece.IsMoveValid(selectedRow, selectedCol, row, col, _board))
                        {
                            // Check if the target square is occupied by an opponent's piece
                            ChessPiece? targetPiece = _board[row, col];
                            if (targetPiece == null || targetPiece.Color != selectedPiece.Color)
                            {
                                // Execute the move by updating the board and the button contents
                                _board[row, col] = selectedPiece;
                                _board[selectedRow, selectedCol] = null;
                                square.Content = _selectedSquare.Content;
                                _selectedSquare.Content = ""; // Clear the previous square

                                // Clear the selection and update the border
                                _selectedSquare.BorderBrush = null;
                                _selectedSquare = null;

                                MessageBox.Show($"Piece moved to {row}, {col}"); // Debug output

                                // Switch the player's turn
                                SwitchTurns();
                            }
                            else
                            {
                                MessageBox.Show("Cannot move to a square occupied by your own piece!"); // Debug output
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid move!"); // Debug output
                        }
                    }
                    else
                    {
                        MessageBox.Show("No piece selected to move!"); // Debug output
                    }

                    // In any case, clear the selected square to reset the UI
                    if (_selectedSquare != null)
                    {
                        _selectedSquare.BorderBrush = null;
                        _selectedSquare = null;
                    }
                }
            }
        }



        private bool IsCheckmate(string playerColor)
        {
            // Iterate through all squares to find all pieces of the current player
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece? piece = _board[row, col];
                    if (piece != null && piece.Color == playerColor)
                    {
                        // Try all potential moves for each piece
                        for (int targetRow = 0; targetRow < 8; targetRow++)
                        {
                            for (int targetCol = 0; targetCol < 8; targetCol++)
                            {
                                // Skip if the move is invalid for the piece
                                if (!piece.IsMoveValid(row, col, targetRow, targetCol, _board))
                                    continue;

                                // Temporarily make the move
                                ChessPiece? originalTargetPiece = _board[targetRow, targetCol];
                                _board[targetRow, targetCol] = piece;
                                _board[row, col] = null;

                                // Check if the king is still in check
                                bool stillInCheck = IsKingInCheck(playerColor);

                                // Undo the move
                                _board[row, col] = piece;
                                _board[targetRow, targetCol] = originalTargetPiece;

                                // If the king is not in check after this move, it's not a checkmate
                                if (!stillInCheck)
                                    return false;
                            }
                        }
                    }
                }
            }
            // If no legal move was found to get the king out of check, it's a checkmate
            return true;
        }



        // Place initial chess pieces
        private void PlacePieces()
        {
            string[] blackPieces = { "♜", "♞", "♝", "♛", "♚", "♝", "♞", "♜" };
            string[] whitePieces = { "♖", "♘", "♗", "♕", "♔", "♗", "♘", "♖" };

            // Place black pieces
            for (int col = 0; col < 8; col++)
            {
                _squares[0, col].Content = blackPieces[col];
                _board[0, col] = CreatePieceBySymbol(blackPieces[col], "Black");

                _squares[1, col].Content = "♟";
                _board[1, col] = new Pawn("Black");
            }

            // Place white pieces
            for (int col = 0; col < 8; col++)
            {
                _squares[7, col].Content = whitePieces[col];
                _board[7, col] = CreatePieceBySymbol(whitePieces[col], "White");

                _squares[6, col].Content = "♙";
                _board[6, col] = new Pawn("White");
            }
        }

        private ChessPiece CreatePieceBySymbol(string symbol, string color)
        {
            return symbol switch
            {
                "♜" => new Rook(color),
                "♞" => new Knight(color),
                "♝" => new Bishop(color),
                "♛" => new Queen(color),
                "♚" => new King(color),
                "♖" => new Rook(color),
                "♘" => new Knight(color),
                "♗" => new Bishop(color),
                "♕" => new Queen(color),
                "♔" => new King(color),
                _ => throw new ArgumentException($"Invalid symbol '{symbol}' for creating a chess piece."),
            };
        }

    }
}
