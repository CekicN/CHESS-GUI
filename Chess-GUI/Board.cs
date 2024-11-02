using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;

namespace Chess_GUI
{
    public class Board
    {
        private Piece[,] pieces = new Piece[8, 8];

        private readonly Dictionary<Player, Position> pawnSkip = new Dictionary<Player, Position>
        {
            {Player.White, null },
            {Player.Black, null }
        };

        public Position GetPawnSkip(Player player)
        {
            return pawnSkip[player];
        }
        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return pieces[pos.Row, pos.Col]; }
            set { pieces[pos.Row, pos.Col] = value; }
        }

        public void setPawnSkip(Player p, Position pos)
        {
            pawnSkip[p] = pos;
        }

        public void removePawnSkip(Player p)
        {
            pawnSkip[p] = null;
        }
        public static Board LoadFenBoard(bool computer)
        {
            Board board = new Board();
            board.Load(computer);
            return board;
        }

        public void LoadFromFen(string fen, bool computer)
        {
            pieces = new Piece[8, 8];
            fen = fen.Split(' ')[0];
            var rows = fen.Split('/');


            for (int i = 0; i < rows.Length; i++)
            {
                int col = 0;
                foreach (char c in rows[i])
                {
                    if (Char.IsDigit(c))
                    {
                        col += (int)Char.GetNumericValue(c);
                    }
                    else
                    {
                        int offset = !computer ? 7 - i : i;
                        switch (c)
                        {
                            case 'r':
                                this[offset, col] = new Rook(Player.White);
                                break;
                            case 'n':
                                this[offset, col] = new Knight(Player.White);
                                break;
                            case 'b':
                                this[offset, col] = new Bishop(Player.White);
                                break;
                            case 'q':
                                this[offset, col] = new Queen(Player.White);
                                break;
                            case 'k':
                                this[offset, col] = new King(Player.White);
                                break;
                            case 'p':
                                this[offset, col] = new Pawn(Player.White, computer);
                                break;

                            case 'R':
                                this[offset, col] = new Rook(Player.Black);
                                break;
                            case 'N':
                                this[offset, col] = new Knight(Player.Black);
                                break;
                            case 'B':
                                this[offset, col] = new Bishop(Player.Black);
                                break;
                            case 'Q':
                                this[offset, col] = new Queen(Player.Black);
                                break;
                            case 'K':
                                this[offset, col] = new King(Player.Black);
                                break;
                            case 'P':
                                this[offset, col] = new Pawn(Player.Black, computer);
                                break;
                        }
                        col++;
                    }
                }
            }
        }
        private void Load(bool computer)
        {
            string fen = File.ReadAllText("..\\..\\files\\input.txt");
            this.LoadFromFen(fen, computer);
        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Col >= 0 && pos.Col < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Position pos = new Position(i, j);
                    if(!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos =>
            {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        public Board Copy()
        {
            Board copy = new Board();

            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }

            return copy;
        }

        public Counting CountPieces()
        {
            Counting counting = new Counting();

            foreach (Position position in PiecePositions())
            {
                Piece piece = this[position];
                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsKingVKing(counting) || IsKingKnightVKing(counting) || IsKingBishopVKing(counting) || IsKingBishopVKingBishop(counting);
        }

        private static bool IsKingVKing(Counting c)
        {
            return c.TotalCount == 2;
        }

        private static bool IsKingBishopVKing(Counting c)
        {
            return c.TotalCount == 3 && (c.White(PieceType.Bishop) == 1 || c.Black(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVKing(Counting c)
        {
            return c.TotalCount == 3 && (c.White(PieceType.Knight) == 1 || c.Black(PieceType.Knight) == 1);
        }
        private bool IsKingBishopVKingBishop(Counting c)
        {
            if (c.TotalCount != 4)
                return false;
            if (c.White(PieceType.Bishop) != 1 || c.Black(PieceType.Bishop) != 1)
                return false;

            Position wBishopPos = FindPiece(Player.White, PieceType.Bishop);
            Position bBishopPos = FindPiece(Player.Black, PieceType.Bishop);

            return wBishopPos.SquareColor() == bBishopPos.SquareColor();
        }

        private Position FindPiece(Player color, PieceType type)
        {
            return PiecePositionsFor(color).First(pos => this[pos].Type == type);
        }

        private bool IsUnmovedKingAndRook(Position kingPos, Position rookPos)
        {
            if(IsEmpty(kingPos) || IsEmpty(rookPos))
            {
                return false;
            }

            Piece king = this[kingPos];
            Piece rook = this[rookPos];

            return king.Type == PieceType.King && rook.Type == PieceType.Rook && !king.HasMoved && !rook.HasMoved;

        }

        public bool CastleRightKS(Player player)
        {
            switch (player)
            {
                case Player.White:
                    return IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7));
                case Player.Black:
                    return IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7));
                default:
                    return false;
            }
        }

        public bool CastleRightQS(Player player)
        {
            switch (player)
            {
                case Player.White:
                    return IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0));
                case Player.Black:
                    return IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0));
                default:
                    return false;
            }
        }

        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPos)
        {
            foreach (Position pos in pawnPositions.Where(IsInside))
            {
                Piece piece = this[pos];
                if(piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
                {
                    continue;
                }

                EnPassant move = new EnPassant(pos, skipPos);
                if(move.IsLegal(this))
                {
                    return true;
                }
            }

            return false;
        }
        public bool CanCaptureEnPassant(Player player)
        {
            Position skipPos = GetPawnSkip(player.Opponent());

            if(skipPos == null)
            {
                return false;
            }

            Position[] pawnPositions;

            switch (player)
            {
                case Player.White:
                    pawnPositions = new Position[] { skipPos + Direction.SouthWest, skipPos + Direction.SouthEast};
                    break;
                case Player.Black:
                    pawnPositions = new Position[] { skipPos + Direction.NorthWest, skipPos + Direction.NorthEast };
                    break;
                default:
                    pawnPositions = Array.Empty<Position>();
                    break;
            }

            return HasPawnInPosition(player, pawnPositions, skipPos);
        }
    }
}
