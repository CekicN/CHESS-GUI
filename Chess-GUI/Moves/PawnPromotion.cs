using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class PawnPromotion : Move
    {
        public override MoveType Type => MoveType.PawnPromotion;

        public override Position FromPos { get; }

        public override Position ToPos { get; }

        private readonly PieceType newType;
        public PawnPromotion(Position from, Position to, PieceType newType)
        {
            this.newType = newType;
            FromPos = from;
            ToPos = to;
        }

        private Piece CreatePromotionPiece(Player color)
        {
            switch (newType)
            {
                case PieceType.Bishop:
                    return new Bishop(color);
                case PieceType.Knight:
                    return new Knight(color);
                case PieceType.Rook:
                    return new Rook(color);
                default:
                    return new Queen(color);
            }
        }
        public override bool Execute(Board board)
        {
            Piece pawn = board[FromPos];
            board[FromPos] = null;

            Piece promotionPiece = CreatePromotionPiece(pawn.Color);
            board[ToPos] = promotionPiece;
            promotionPiece.HasMoved = true;

            return true;
        }

        public override bool backMove(Board board, bool computer)
        {
            board[FromPos] = new Pawn(board[ToPos].Color, computer);
            board[ToPos] = null;

            return true;
        }
    }
}
