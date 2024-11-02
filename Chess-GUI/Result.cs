using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class Result
    {
        public Player Winner { get; }
        public EndReason Reason { get; }
        public Result(Player winner, EndReason reason)
        {
            Winner = winner;
            Reason = reason;
        }   

        public static Result Win(Player player, EndReason reason)
        {
            return new Result(player, reason);
        }

        public static Result Draw(EndReason reason)
        {
            return new Result(Player.None, reason);
        }
    }
}
