using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;

        public override Player Color { get; }

        private readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
            Direction.NorthEast,
            Direction.NorthWest,
            Direction.SouthEast,
            Direction.SouthWest
        };
        public King(Player color)
        {
            Color = color;
        }

        private static bool IsUnmovedRook(Position pos, Board board)
        {
            if (board.IsEmpty(pos))
                return false;
            Piece piece = board[pos];
            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }

        private static bool AllEmpty(IEnumerable<Position> positions, Board board)
        {
            return positions.All(pos => board.IsEmpty(pos));
        }

        private bool CanRokadaKS(Position from ,Board board)
        {
            if(HasMoved)
            {
                return false;
            }

            Position rookPos = new Position(from.Row, 7);
            Position[] between = new Position[] { new Position(from.Row, 5), new Position(from.Row, 6) };
            return IsUnmovedRook(rookPos, board) && AllEmpty(between, board);
        }

        private bool CanRokadaQS(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPos = new Position(from.Row, 0);
            Position[] between = new Position[] { new Position(from.Row, 1), new Position(from.Row, 2), new Position(from.Row, 3) };

            return IsUnmovedRook(rookPos, board) && AllEmpty(between, board);
        }
        public override Piece Copy()
        {
            King cp = new King(Color);
            cp.HasMoved = HasMoved;
            return cp;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in PotentionalMoves(from, board))
            {
                yield return new NormalMove(from, to);
            }

            if(CanRokadaKS(from, board))
            {
                yield return new RokadaMove(MoveType.CastleKS, from);
            }

            if (CanRokadaQS(from, board))
            {
                yield return new RokadaMove(MoveType.CastleQS, from);
            }
        }

        private IEnumerable<Position> PotentionalMoves(Position from, Board board)
        {
            foreach (Direction dir in dirs)
            {
                Position to = from + dir;

                if (!Board.IsInside(to))
                    continue;

                if(board.IsEmpty(to) || board[to].Color != Color)
                    yield return to;
            }
        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return PotentionalMoves(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
