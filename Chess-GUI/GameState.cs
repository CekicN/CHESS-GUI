using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace Chess_GUI
{
    public class GameState
    {
        public Board Board { get; set; }
        public Player CurrentPlayer { get; set; }
        public Result Result { get; private set; } = null;

        private int noCapOrPawnMoves = 0;
        private string stateString;
        private readonly Dictionary<string, int> stateHistory = new Dictionary<string, int>();

        private Player topPlayer;
        public TimeSpan timer { get; set; }
        public GameState(Player player, Board board, Player p)
        {
            CurrentPlayer = player;
            Board = board;
            stateString = new StateString(CurrentPlayer, board, p).ToString();
            topPlayer = p;
            stateHistory[stateString] = 1;
        }

        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            if(Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }
            Piece piece = Board[pos];
            IEnumerable<Move> moves = piece.GetMoves(pos, Board);
            return moves.Where(move => move.IsLegal(Board));
        }

        public void MakeMove(Move move)
        {
            Board.setPawnSkip(CurrentPlayer, null);
            if (move.Execute(Board))
            {
                noCapOrPawnMoves = 0;
                stateHistory.Clear();
            }
            else
                noCapOrPawnMoves++;

            CurrentPlayer = CurrentPlayer.Opponent();
            UpdateStateString();
            CheckForGameOver();
        }

        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos =>
            {
                Piece piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        private void CheckForGameOver()
        {
            if(!AllLegalMovesFor(CurrentPlayer).Any())
            {
                if(Board.IsInCheck(CurrentPlayer))
                {
                    Result = Result.Win(CurrentPlayer.Opponent(), EndReason.Checkmate);
                }
                else
                {
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
            else if (TimeExpired())
            {
                Result = Result.Win(CurrentPlayer.Opponent(), EndReason.Timer);
            }
            else if(Board.InsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }
            else if(FiftyMoveRule())
            {
                Result = Result.Draw(EndReason.FiftyMoveRule);
            }
            else if(Threefold())
            {
                Result = Result.Draw(EndReason.ThreefoldRepetation);
            }
        }

        public bool TimeExpired()
        {
            return timer.Seconds <= 0;
        }
        public bool IsGameOver()
        {
            return Result != null;
        }

        private bool FiftyMoveRule()
        {
            int fullMoves = noCapOrPawnMoves / 2;

            return fullMoves == 50;
        }

        private void UpdateStateString()
        {
            stateString = new StateString(CurrentPlayer, Board, topPlayer).ToString();

            if (!stateHistory.ContainsKey(stateString))
                stateHistory[stateString] = 1;
            else
                stateHistory[stateString]++;

        }

        private bool Threefold()
        {
            return stateHistory[stateString] == 3;
        }
    }
}
