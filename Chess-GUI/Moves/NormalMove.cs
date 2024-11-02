using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;

        public override Position FromPos { get; }

        public override Position ToPos { get; }

        private Piece eatedPiece { get; set; }
        public NormalMove(Position from, Position to)
        {
            FromPos = from;
            ToPos = to; 
        }
        public override bool Execute(Board board)
        {
            Piece piece = board[FromPos];
            bool capture = !board.IsEmpty(ToPos);
            if (capture)
            {
                SoundPlayer sound = new SoundPlayer(@"D:\Faks\Diplomski CHESS-GUI\CHESS-GUI\Chess-GUI\Sounds\capture.wav");
                sound.Load();
                sound.Play();
            }
            eatedPiece = board[ToPos];
            board[ToPos] = piece;
            board[FromPos] = null;
            piece.HasMoved = true;

            return capture || piece.Type == PieceType.Pawn;
        }

        public override bool backMove(Board board, bool computer)
        {
            board[FromPos] = board[ToPos];
            board[FromPos].HasMoved = false;
            board[ToPos] = eatedPiece;

            return true;
        }
    }
}
