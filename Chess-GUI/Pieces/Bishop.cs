using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class Bishop : Piece
    {
        public override PieceType Type => PieceType.Bishop;

        public override Player Color { get; }

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.NorthEast,
            Direction.NorthWest,
            Direction.SouthEast,
            Direction.SouthWest
        };

        public Bishop(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Bishop cp = new Bishop(Color);
            cp.HasMoved = HasMoved;
            return cp;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
