using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class EnPassant : Move
    {
        public override MoveType Type => MoveType.EnPassant;

        public override Position FromPos { get; }

        public override Position ToPos { get; }

        private readonly Position capturePos;
        private Piece eatedPiece { get; set; }
        public EnPassant(Position fromPos, Position toPos)
        {
            FromPos = fromPos;
            ToPos = toPos;
            this.capturePos = new Position(FromPos.Row, toPos.Col);
        }

        public override bool Execute(Board board)
        {
            eatedPiece = board[capturePos];
            new NormalMove(FromPos, ToPos).Execute(board);
            board[capturePos] = null;

            return true;
        }

        public override bool backMove(Board board, bool computer)
        {
            new NormalMove(FromPos, ToPos).backMove(board, computer);
            board[capturePos] = eatedPiece;

            return true;
        }
    }
}
