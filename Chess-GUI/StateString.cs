using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class StateString
    {
        private readonly StringBuilder sb = new StringBuilder();

        public StateString(Player currentPlayer, Board board, Player p)
        {
            AddPiecePlacement(board, p);
            sb.Append(' ');
            AddCurrentPlayer(currentPlayer);
            sb.Append(' ');
            AddCastlingRights(board);
            sb.Append(' ');
            AddEnPassant(board, currentPlayer);
            
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        private static char PieceChar(Piece piece, Player topPlayer)
        {
            char c;

            switch (piece.Type)
            {
                case PieceType.Pawn:
                    c = 'p';
                    break;
                case PieceType.Bishop:
                    c = 'b';
                    break;
                case PieceType.Knight:
                    c = 'n';
                    break;
                case PieceType.Rook:
                    c = 'r';
                    break;
                case PieceType.Queen:
                    c = 'q';
                    break;
                case PieceType.King:
                    c = 'k';
                    break;
                default:
                    c = ' ';
                    break;
            }

            if(piece.Color != topPlayer)
            {
                return char.ToUpper(c);
            }

            return c;
        }

        private void AddRowData(Board board, int row, Player topPlayer)
        {
            int empty = 0;

            for (int i = 0; i < 8; i++)
            {
                if (board[row, i] == null)
                {
                    empty++;
                    continue;
                }

                if(empty > 0)
                {
                    sb.Append(empty);
                    empty = 0;
                }

                sb.Append(PieceChar(board[row, i], topPlayer));
            }

            if(empty > 0)
            {
                sb.Append(empty);
            }
        }

        private void AddPiecePlacement(Board board, Player topPlayer)
        {
            for (int i = 0; i < 8; i++)
            {
                if(i != 0)
                {
                    sb.Append('/');
                }

                AddRowData(board, i, topPlayer);
            }
        }

        private void AddCurrentPlayer(Player currentPlayer)
        {
            if(currentPlayer == Player.White)
            {
                sb.Append('w');
            }
            else
                sb.Append('b');
        }


        private void AddCastlingRights(Board board)
        {
            bool castleWKS = board.CastleRightKS(Player.White);
            bool castleWQS = board.CastleRightQS(Player.White);
            bool castleBKS = board.CastleRightKS(Player.Black);
            bool castleBQS = board.CastleRightQS(Player.Black);

            if(!(castleWKS || castleWQS || castleBKS || castleBQS))
            {
                sb.Append('-');
                return;
            }

            if(castleWKS)
            {
                sb.Append('K');
            }

            if (castleWQS)
            {
                sb.Append('Q');
            }

            if (castleBKS)
            {
                sb.Append('k');
            }

            if (castleBQS)
            {
                sb.Append('q');
            }
        }

        private void AddEnPassant(Board board, Player currentPlayer)
        {
            if(!board.CanCaptureEnPassant(currentPlayer))
            {
                sb.Append('-');
                return;
            }

            Position pos = board.GetPawnSkip(currentPlayer.Opponent());

            char file = (char)('a' + pos.Col);
            int rank = 8 - pos.Row;
            sb.Append(file);
            sb.Append(rank);
        }

        public string readFromEnd()
        {
            string fen = sb.ToString().Split(' ')[0];
            string ostatak = sb.ToString().Substring(sb.ToString().IndexOf(' '));
            char[] charMove = new char[fen.Length];
            int j = 0;

            string[] rows = fen.Split('/');
            for (int i = rows.Length - 1; i >= 0; i--)
            {
                for(int k = 0; k < rows[i].Length; k++)
                {
                    if (char.IsUpper(rows[i][k]))
                        charMove[j++] = char.ToLower(rows[i][k]);
                    else if (!char.IsUpper(rows[i][k]))
                        charMove[j++] = char.ToUpper(rows[i][k]);
                    else if (char.IsDigit(rows[i][k]))
                        charMove[j++] = rows[i][k];
                }
                if(j < fen.Length)
                    charMove[j++] = '/';
            }

            string kraj = new string(charMove);
            kraj +=  ostatak;
            return kraj;
        }
    }
}
