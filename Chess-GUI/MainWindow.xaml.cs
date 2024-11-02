using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Chess_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private Color WhiteColor = Color.FromRgb(18, 101, 255);
        private Color BlackColor = Color.FromRgb(189, 219, 230);

        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
        private readonly List<Position> positions = new List<Position>();

        private MovesHistory movesHistory = new MovesHistory();
        private MovesHistory movesFromFile = new MovesHistory();

        private GameState gameState;
        private Position selectedPos = null;
        private JoystickHandler handler;
        private Stockfish stockfishEngine;

        private DispatcherTimer timer;
        private TimeSpan time;

        private bool computer = false;
        private Player topPlayer = Player.Black;

        private SoundPlayer moveSound;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            stockfishEngine = new Stockfish();
            handler = new JoystickHandler(this);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Tick;

            moveSound = new SoundPlayer(@"D:\Faks\Diplomski CHESS-GUI\CHESS-GUI\Chess-GUI\Sounds\move-self.wav");
            moveSound.Load();

        }

        private void Tick(object sender, EventArgs e)
        {
            if (time.TotalSeconds > 0)
            {
                time = time.Add(TimeSpan.FromSeconds(-1));
                gameState.timer = time;
                Timer.Text = time.ToString(@"mm\:ss");
            }
            else
            {
                timer.Stop();
            }
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeBoard();

            gameState = new GameState(Player.White, Board.LoadFenBoard(computer), topPlayer);
            DrawBoard();
            DrawPieces(gameState.Board);
            stockfishEngine.Start();
            time = TimeSpan.FromMinutes(5);
            gameState.timer = time;
            Timer.Text = time.ToString(@"mm\:ss");

            if(!computer)
                timer.Start();
            await handler.StartTracking();

            
        }
        
        protected override void OnClosed(EventArgs e)
        {
            handler.StartTracking();
            stockfishEngine.Stop();
            base.OnClosed(e);
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image img = new Image();
                    pieceImages[i, j] = img;
                    img.Margin = new Thickness(10);
                    PieceGrid.Children.Add(img);

                    Rectangle light = new Rectangle();
                    highlights[i, j] = light;
                    HighlightGrid.Children.Add(light);
                }
            }
        }
        private void ColorPicker1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (ColorPicker1.SelectedColor.HasValue)
            {
                WhiteColor = (Color)ColorPicker1.SelectedColor;
                DrawBoard();
            }
        }

        private void ColorPicker2_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (ColorPicker2.SelectedColor.HasValue)
            {
                BlackColor = (Color)ColorPicker2.SelectedColor;
                DrawBoard();
            }

        }

        private void DrawBoard()
        {
            double squareSize = 100;
            if (canvas == null) return;
            canvas.Children.Clear();
            Color color;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if ((i + j) % 2 == 0)
                        color = WhiteColor;
                    else
                        color = BlackColor;

                    var rect = new Rectangle
                    {
                        Width = squareSize,
                        Height = squareSize,
                        Fill = new SolidColorBrush(color)
                    };

                    Canvas.SetLeft(rect, j * squareSize);
                    Canvas.SetTop(rect, i * squareSize);

                    canvas.Children.Add(rect);
                }
            }

            var r = new Rectangle
            {
                Width = 30,
                Height = 830,
                Fill = new SolidColorBrush(Colors.White)
            };
            Canvas.SetTop(r, 20);
            Canvas.SetLeft(r, 800);

            canvas.Children.Add(r);

            r = new Rectangle
            {
                Width = 800,
                Height = 30,
                Fill = new SolidColorBrush(Colors.White)
            };

            Canvas.SetTop(r, 800);
            Canvas.SetLeft(r, 0);

            canvas.Children.Add(r);

            for (int i = 0; i < 8; i++)
            {
                var t = new TextBlock
                {
                    Text = (8 - i).ToString(),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold
                };

                Canvas.SetLeft(t, 805);
                Canvas.SetTop(t, (i * 100) + 40);
                canvas.Children.Add(t);


                t = new TextBlock
                {
                    Text = ((char)(i + (int)'a')).ToString(),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold
                };

                Canvas.SetTop(t, 797);
                Canvas.SetLeft(t, (i * 100) + 45);
                canvas.Children.Add(t);

            }
        }

        private void DrawPieces(Board board)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = board[i, j];
                    pieceImages[i, j].Source = Images.GetImage(piece);
                }
            }
        }


        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();

            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Col].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Col].Fill = Brushes.Transparent;
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuOnScreen() || rb2.IsChecked != true)
            {
                return;
            }
            if(gameState.CurrentPlayer != topPlayer)
            {
                Point point = e.GetPosition(BoardGrid);
                Position pos = toSqarePosition(point);

                odigraj(pos);
            }
        }
       
        public void odigraj(Position pos)
        {
            if (selectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }
        private void OnFromPositionSelected(Position pos)
        {
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);

            if (moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }
        private void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();
            moveSound.Play();
            if (moveCache.TryGetValue(pos, out Move move))
            {
                if (move.Type == MoveType.PawnPromotion)
                {
                    HandlePromotion(move.FromPos, move.ToPos);
                }
                else
                    HandleMove(move);
            }
        }

        private void HandlePromotion(Position FromPos, Position ToPos)
        {
            if(gameState.CurrentPlayer != topPlayer)
            {
                pieceImages[ToPos.Row, ToPos.Col].Source = Images.GetImage(gameState.CurrentPlayer, PieceType.Pawn);
                pieceImages[FromPos.Row, FromPos.Col].Source = null;

                PromotionMenu promMenu = new PromotionMenu(gameState.CurrentPlayer);

                MenuContainer.Content = promMenu;

                promMenu.PieceSelected += type =>
                {
                    MenuContainer.Content = null;

                    Move promMove = new PawnPromotion(FromPos, ToPos, type);
                    HandleMove(promMove);
                };
            }
            else
            {
                Move promMove = new PawnPromotion(FromPos, ToPos, PieceType.Queen);
                HandleMove(promMove);
            }
        }

        private async void BlackMove()
        {
            if(gameState.CurrentPlayer == topPlayer)
            {
                InfoTextBlock.Text = "";
                string move = "";
                if (topPlayer != Player.White)
                    move = await stockfishEngine.BestMoves(new StateString(gameState.CurrentPlayer, gameState.Board, topPlayer).ToString(), InfoTextBlock, scroll);
                else
                    move = await stockfishEngine.BestMoves(new StateString(gameState.CurrentPlayer, gameState.Board, topPlayer).readFromEnd(), InfoTextBlock, scroll);
                if (move == "(non")
                    return;

                if(topPlayer == Player.White)
                {
                    char[] charMove = move.ToCharArray();
                    charMove[1] = char.Parse(('9' -  charMove[1]).ToString());
                    charMove[3] = char.Parse(('9' - charMove[3]).ToString());

                    move = new string(charMove);
                }
                int fromCol = move[0] - 'a';
                int fromRow = 8 - Int32.Parse(move[1].ToString());
                int toCol = move[2] - 'a';
                int toRow = 8 - Int32.Parse(move[3].ToString());

                OnFromPositionSelected(new Position(fromRow, fromCol));

                await Task.Delay(500);

                OnToPositionSelected(new Position(toRow, toCol));
            }
        }
        private void HandleMove(Move move)
        {
            movesHistory.addMove(move);
            gameState.MakeMove(move);
            DrawPieces(gameState.Board);

            if (gameState.IsGameOver())
            {
                ShowGameOver();
            }
            if (gameState.CurrentPlayer == topPlayer)
            {
                timer.Stop();
                BlackMove();
            }
            if (gameState.CurrentPlayer != topPlayer)
                timer.Start();
        }
        private Position toSqarePosition(Point p)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(p.Y / squareSize);
            int col = (int)(p.X / squareSize);

            return new Position(row, col);
        }

        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }

        private void ShowGameOver()
        {
            GameOverMenu gameOver = new GameOverMenu(gameState);
            MenuContainer.Content = gameOver;

            gameOver.OptionSelected += option =>
            {
                if(option == Option.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            };
        }

        private void RestartGame()
        {
            HideHighlights();
            moveCache.Clear();
            gameState = new GameState(Player.White, Board.LoadFenBoard(computer), topPlayer);
            time = TimeSpan.FromMinutes(5);
            DrawPieces(gameState.Board);
            if (computer)
                BlackMove();
        }
        private void radioChecked(object sender, RoutedEventArgs e)
        {
            RadioButton selectedBtn = sender as RadioButton;

            if (selectedBtn.Name == "rb1")
            {
                RestartGame();
                string[] lines = File.ReadAllLines("..\\..\\files\\moves.txt");
                Board tabla = gameState.Board.Copy();
                foreach (var move in lines)
                {
                    int fromCol = move[0] - 'a';
                    int fromRow = 8 - Int32.Parse(move[1].ToString());
                    int toCol = move[2] - 'a';
                    int toRow = 8 - Int32.Parse(move[3].ToString());

                    IEnumerable<Move> moves = gameState.LegalMovesForPiece(new Position(fromRow, fromCol));
                    if(moves.Any())
                        CacheMoves(moves);

                    if (moveCache.TryGetValue(new Position(toRow, toCol), out Move potez))
                    {
                        if (potez.Type == MoveType.PawnPromotion)
                            potez = new PawnPromotion(new Position(fromRow, fromCol), new Position(fromRow, fromCol), PieceType.Queen);
                        movesFromFile.addMove(potez);
                        gameState.MakeMove(potez);
                    }
                }
                movesFromFile.index = -1;
                gameState.Board = tabla;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (rb1.IsChecked == false)
            {
                movesHistory.Back(gameState.Board, computer);
            }
            else
            {
                movesFromFile.Back(gameState.Board, computer);
            }
            DrawPieces(gameState.Board);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (rb1.IsChecked == false)
            {
                movesHistory.Forward(gameState.Board);
            }
            else
            {
                movesFromFile.Forward(gameState.Board);
            }
            DrawPieces(gameState.Board);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            RestartGame();
        }
        private void Rotate_Click(object sender, RoutedEventArgs e)
        {
            computer = !computer;
            topPlayer = topPlayer == Player.White ? Player.Black:Player.White;
            RestartGame();
        }
    }
}
