using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class RokadaMove : Move
    {
        public override MoveType Type { get; }

        public override Position FromPos { get; }

        public override Position ToPos { get; }

        private readonly Direction kingMoveDir;
        private readonly Position rookFromPos;
        private readonly Position rookToPos;

        public RokadaMove(MoveType type, Position kingPos)
        {
            Type = type;
            FromPos = kingPos;

            if (type == MoveType.CastleKS)
            {
                kingMoveDir = Direction.East;
                ToPos = new Position(kingPos.Row, 6);
                rookFromPos = new Position(kingPos.Row, 7);
                rookToPos = new Position(kingPos.Row, 5);
            }
            else if(type == MoveType.CastleQS)
            {
                kingMoveDir = Direction.West;
                ToPos = new Position(kingPos.Row, 2);
                rookFromPos = new Position(kingPos.Row, 0);
                rookToPos = new Position(kingPos.Row, 3);
            }
        }
        public override bool Execute(Board board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);
            new NormalMove(rookFromPos, rookToPos).Execute(board);

            return false;
        }

        public override bool IsLegal(Board board)
        {
            Player player = board[FromPos].Color;
            if (board.IsInCheck(player))
                return false;

            Board cpy = board.Copy();
            Position kingPos = FromPos;

            for(int i = 0; i < 2; i++)
            {
                new NormalMove(kingPos, kingPos + kingMoveDir).Execute(cpy);
                kingPos += kingMoveDir;

                if (cpy.IsInCheck(player))
                    return false;
            }
            return true;
        }

        public override bool backMove(Board board, bool computer)
        {
            new NormalMove(FromPos, ToPos).backMove(board, computer);
            new NormalMove(rookFromPos, rookToPos).backMove(board, computer);

            return false;
        }
    }
}
