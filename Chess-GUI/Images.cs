using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess_GUI
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new Dictionary<PieceType, ImageSource>
        {
            {PieceType.Pawn, LoadImage("assets/w_pawn_png_512px.png") },
            {PieceType.Rook, LoadImage("assets/w_rook_png_512px.png") },
            {PieceType.King, LoadImage("assets/w_king_png_512px.png") },
            {PieceType.Queen, LoadImage("assets/w_queen_png_512px.png") },
            {PieceType.Knight, LoadImage("assets/w_knight_png_512px.png") },
            {PieceType.Bishop, LoadImage("assets/w_bishop_png_512px.png") }
        };
        private static readonly Dictionary<PieceType, ImageSource> blackSources = new Dictionary<PieceType, ImageSource>
        {
            {PieceType.Pawn, LoadImage("assets/b_pawn_png_512px.png") },
            {PieceType.Rook, LoadImage("assets/b_rook_png_512px.png") },
            {PieceType.King, LoadImage("assets/b_king_png_512px.png") },
            {PieceType.Queen, LoadImage("assets/b_queen_png_512px.png") },
            {PieceType.Knight, LoadImage("assets/b_knight_png_512px.png") },
            {PieceType.Bishop, LoadImage("assets/b_bishop_png_512px.png") }
        };
        private static ImageSource LoadImage(string filename )
        {
            return new BitmapImage(new Uri(filename, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PieceType piece)
        {
            switch(color)
            {
                case Player.White:
                    return whiteSources[piece];
                case Player.Black:
                    return blackSources[piece];
                default:
                    return null;
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
                return null;

            return GetImage(piece.Color, piece.Type);
        }
    }
}
