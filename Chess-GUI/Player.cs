using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public enum Player
    {
        None,
        White,
        Black
    }
    public static class PlayerExt
    {
        public static Player Opponent(this Player player)
        {
            switch(player)
            {
                case Player.White:
                    return Player.Black;
                case Player.Black:
                    return Player.White;
                default:
                    return Player.None;
            }
        }
    }
}
