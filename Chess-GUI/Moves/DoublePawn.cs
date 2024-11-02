using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class DoublePawn : Move
    {
        public override MoveType Type => MoveType.DoublePawn;

        public override Position FromPos { get; }

        public override Position ToPos { get; }

        private readonly Position skippedPos;
        public DoublePawn(Position From, Position To)
        {
            FromPos = From;
            ToPos = To;
            this.skippedPos = new Position((From.Row + To.Row)/2, From.Col);
        }
        public override bool Execute(Board board)
        {
            Player player = board[FromPos].Color;
            board.setPawnSkip(player, skippedPos);
            new NormalMove(FromPos, ToPos).Execute(board);

            return true;
        }

        public override bool backMove(Board board, bool computer)
        {
            Player player = board[ToPos].Color;
            board.removePawnSkip(player);
            new NormalMove(FromPos, ToPos).backMove(board, computer);

            return true;
        }
    }
}
