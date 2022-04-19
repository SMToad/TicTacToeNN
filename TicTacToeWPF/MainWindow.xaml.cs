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
using System.Windows.Threading;
using OxyPlot;
using TicTacToeWPF.GameElements;
using TicTacToeWPF.Models;
using TicTacToeWPF.Players;

namespace TicTacToeWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        GameModel gameModel;
        Game game;
        InteractiveGame gameInteractive;
        DispatcherTimer timer;

        AppViewModel appViewModel;
        GraphViewModel graphViewModel;
        TrainStats trainStats;


        public MainWindow()
        {
            InitializeComponent();
            gameModel = new GameModel()
            {
                PlayBoard = new PlayBoard(),
                Players = new List<Player>(2)
            };

            appViewModel = new AppViewModel()
            {
                Person = new Person(),
                TrainOpponent = new RandomPlayer(),
                TrainRounds = 50
            };
            gameModel.Players.Add(new Agent());
            gameModel.Players.Add(appViewModel.Person);

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(0.001);

            cbOpponent.ItemsSource = new List<string>() { "Рандом", "Подряд", "Сама с собой" };

            graphViewModel = new GraphViewModel()
            {
                NetWinsData = new List<DataPoint> { new DataPoint(0, 0) },
                NetLossesData = new List<DataPoint> { new DataPoint(0, 0) },
                TiesData = new List<DataPoint> { new DataPoint(0, 0) },
            };

            lsNetWinsData.ItemsSource = graphViewModel.NetWinsData;
            lsNetLossesData.ItemsSource = graphViewModel.NetLossesData;
            lsTiesData.ItemsSource = graphViewModel.TiesData;

            trainStats = new TrainStats();
            InitializeGrid();
        }

        void UpdateVisualStats(GameState gameState)
        {
            trainStats.Increment(gameState);
            tbRoundsCount.Text = "Раунд " + trainStats.RoundsCount;

            int[,] board = gameModel.PlayBoard.GetBoard();
            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                    if(board[i,j] != 0)
                        PlaceMoveOnGrid((i, j), (PlayerTurn)board[i, j]);

            if (trainStats.RoundsCount % appViewModel.TrainRounds == 0)
            {
                if (!gameModel.GetAgent().InTraining)
                {
                    StatsViewModel stats = new StatsViewModel(trainStats, appViewModel.TrainRounds);
                    lvResultTable.Items.Add(stats);
                }
                gameModel.GetAgent().ToggleTraining();
                graphViewModel.Add(trainStats, appViewModel.TrainRounds);
                pltResultGraph.InvalidatePlot(true);
                trainStats.ClearResults();
            }
            if (trainStats.RoundsCount == appViewModel.TotalRounds)
            {
                timer.Stop();
                lbStatusBar.Content = "Обучение завершено. ";
            }
        }
        public void ClearVisualStats()
        {
            tbRoundsCount.Text = "";
            trainStats.Clear();
            graphViewModel.Clear();
            pltResultGraph.InvalidatePlot();
            lvResultTable.Items.Clear();
        }
        void InitializeGrid()
        {
            gridPlayBoard.Children.Clear();
            gridPlayBoard.ColumnDefinitions.Clear();
            gridPlayBoard.RowDefinitions.Clear();
            int size = gameModel.PlayBoard.Size;
            for (int i = 0; i < size; i++)
            {
                gridPlayBoard.RowDefinitions.Add(new RowDefinition());
                gridPlayBoard.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    Button button = new Button();
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    button.FontSize = 18;
                    button.IsHitTestVisible = false;
                    button.Click += btnGrid_Click;
                    gridPlayBoard.Children.Add(button);
                }
        }
        void AllowGridClick(bool isGame)
        {
            foreach (Button btn in gridMain.Children.OfType<Button>())
                btn.IsHitTestVisible = !isGame;
            foreach (Button btn in gridPlayBoard.Children)
                btn.IsHitTestVisible = isGame;
        }
        void ClearGridContent()
        {
            foreach (Button btn in gridPlayBoard.Children)
                btn.Content = "";
        }
        void btnGrid_Click(object sender, RoutedEventArgs args)
        {
            Button btn = sender as Button;
            if (btn.Content != null && btn.Content.ToString() != "") return;
            (int X, int Y) move = (Grid.GetRow(btn), Grid.GetColumn(btn));
            appViewModel.Person.LastMove = move;
            PlaceMoveOnGrid(move, gameModel.CurrentTurn);
            btn.Content = gameModel.CurrentTurn;
            lbStatusBar.Content = "Ваш ход сделан. Ожидание хода сети...";
            gameInteractive.EndMove();
            UpdateInteractiveStats();
        }

        void cbOpponent_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            switch (cbOpponent.SelectedIndex)
            {
                case 0:
                    appViewModel.TrainOpponent = new RandomPlayer();
                    break;
                case 1:
                    appViewModel.TrainOpponent = new BoringPlayer();
                    break;
                case 2:
                    appViewModel.TrainOpponent = gameModel.GetAgent();
                    break;
            }
        }
        void PlaceMoveOnGrid((int X, int Y) move, PlayerTurn playerTurn)
        {
            foreach (Button btn in gridPlayBoard.Children)
                if (Grid.GetRow(btn) == move.X && Grid.GetColumn(btn) == move.Y)
                    btn.Content = playerTurn;
            
        }
        void UpdateInteractiveStats()
        {
            List<(int X, int Y)> playedMoves = gameModel.PlayBoard.PlayedMoves;
            List<int> playedValues = gameModel.PlayBoard.PlayedValues;
            if(gameModel.GameState != GameState.InGame)
            {
                if (gameModel.Players.First() is Agent)
                    PlaceMoveOnGrid(playedMoves.Last(), (PlayerTurn)playedValues.Last());
                AllowGridClick(false);
                gameModel.SwitchPlayers();
            }
            else if (gameModel.PlayBoard.PlayedMoves.Count > 0)
            {
                PlaceMoveOnGrid(playedMoves.Last(), (PlayerTurn)playedValues.Last());
                lbStatusBar.Content = "Сеть сделала свой выбор. ";
            }
            lbStatusBar.Content += appViewModel.Person.StatusBarLabel;
            
            
        }
        void timer_Tick(object sender, EventArgs args)
        {
            ClearGridContent();
            gameModel.PlayBoard = new PlayBoard(gameModel.PlayBoard.Size);
            GameState gameState = game.Play();
            gameModel.SwitchPlayers();
            UpdateVisualStats(gameState);
        }
        void btnResetNet_Clicked(object sender, RoutedEventArgs args)
        {
            ResetNet();
        }
        void ResetNet()
        {
            gameModel.SetAgent(new Agent(gameModel.PlayBoard.Size));
            lbStatusBar.Content = "Сеть обновлена. ";
        }
        int GetPlayBoardSizeInput()
        {
            if (tbPlayBoardSize.Text == "")
            {
                lbStatusBar.Content = "Задайте размер поля!";
                return gameModel.PlayBoard.Size;
            }
            return Convert.ToInt32(tbPlayBoardSize.Text);
        }
        void UpdateTotalRounds()
        {
            if (tbTotalRounds.Text == "")
            {
                lbStatusBar.Content = "Задайте количество тренировочных игр!";
            }
            else appViewModel.TotalRounds = Convert.ToInt32(tbTotalRounds.Text) * 2;
        }
        void UpdatePlayBoardSize()
        {
            int boardSize = GetPlayBoardSizeInput();
            if (gameModel.PlayBoard.Size != boardSize)
            {
                gameModel.PlayBoard.Size = boardSize;
                ResetNet();
                InitializeGrid();
            }
        }
        void btnStartGame_Clicked(object sender, RoutedEventArgs args)
        {
            UpdatePlayBoardSize();
            ClearGridContent();
            AllowGridClick(true);
            gameModel.PlayBoard = new PlayBoard(gameModel.PlayBoard.Size);
            if (!gameModel.Players.Any(p => p is Person))
                gameModel.Players[1] = appViewModel.Person;
            gameInteractive = new InteractiveGame()
            {
                GameModel = gameModel
            };
            gameInteractive.StartGame();
            lbStatusBar.Content = appViewModel.Person.StatusBarLabel;
            UpdateInteractiveStats();
        }
        void btnStartTrain_Clicked(object sender, RoutedEventArgs args)
        {
            ClearVisualStats();
            UpdatePlayBoardSize();
            UpdateTotalRounds();
            gameModel.Players[0] = gameModel.GetAgent();
            gameModel.Players[1] = appViewModel.TrainOpponent;
            game = new Game()
            {
                GameModel = gameModel
            };
            lbStatusBar.Content = "Обучение сети в процессе. ";
            timer.Start();
        }
        void btnStop_Clicked(object sender, RoutedEventArgs args)
        {

            timer.Stop();
            lbStatusBar.Content = "Обучение завершено. ";

        }
    }
}
