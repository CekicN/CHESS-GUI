using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess_GUI
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> OptionSelected;
        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();

            Result result = gameState.Result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = getReasonText(result.Reason, gameState.CurrentPlayer);
        }

        private static string GetWinnerText(Player player)
        {
            switch (player)
            {   
                case Player.White:
                    return "White wins";
                case Player.Black:
                    return "Black wins";
                default:
                    return "It's a draw";
            }
        }

        private static string PlayerString(Player player)
        {
            switch (player)
            {
                case Player.White:
                    return "WHITE";
                case Player.Black:
                    return "BLACK";
                default:
                    return "";
            }
        }

        private static string getReasonText(EndReason reason, Player current)
        {
            switch (reason)
            {
                case EndReason.Checkmate:
                    return $"Checkmate - {PlayerString(current)} Can't Move";
                case EndReason.Stalemate:
                    return $"Stalemate - {PlayerString(current)} Can't Move";
                case EndReason.FiftyMoveRule:
                    return $"FiftyMoveRule";
                case EndReason.InsufficientMaterial:
                    return $"InsufficientMaterial";
                case EndReason.ThreefoldRepetation:
                    return $"ThreefoldRepetation";
                case EndReason.Timer:
                    return "Expired Time";
                default:
                    return "";
            }
        }
        private void RestartClick(object sender, RoutedEventArgs e)
        {
            OptionSelected.Invoke(Option.Restart);
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            OptionSelected.Invoke(Option.Exit);
        }
    }
}
