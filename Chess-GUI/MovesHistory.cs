using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public class MovesHistory
    {
        private List<Move> movesHistory;
        public int index { get; set; }
        public MovesHistory() 
        {
            movesHistory = new List<Move>();
            index = -1;
        }

        public void addMove(Move move)
        {
            
            if (index < movesHistory.Count - 1)
            {
                movesHistory.RemoveRange(index+1, movesHistory.Count - index - 1);
            }
            movesHistory.Add(move);
            index++;
        }

        public void Back(Board board, bool computer)
        {
            if(index >= 0)
            {
                Move move = movesHistory[index--];

                move.backMove(board, computer);
            }
        }

        public void Forward(Board board)
        {
            if(index < movesHistory.Count - 1)
            {
                Move move = movesHistory[++index];
                move.Execute(board);
            }
        }
    }
}
