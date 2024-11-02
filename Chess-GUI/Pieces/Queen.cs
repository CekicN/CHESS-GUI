using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class Queen:Piece
    {
        public override PieceType Type => PieceType.Queen;

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
        public Queen(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Queen cp = new Queen(Color);
            cp.HasMoved = HasMoved;
            return cp;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
