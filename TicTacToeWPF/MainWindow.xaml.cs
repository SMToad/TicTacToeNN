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

        InputViewModel inputViewModel;
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

            inputViewModel = new InputViewModel();
            DataContext = inputViewModel;
            gameModel.Players.Add(new Agent());
            gameModel.Players.Add(new Person());

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(0.01);


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
        void btnStartGame_Clicked(object sender, RoutedEventArgs args)
        {
            UpdatePlayBoardSize();
            ClearGridContent();
            AllowGridClick(true);
            gameModel.PlayBoard = new PlayBoard(gameModel.PlayBoard.Size);
            if (!gameModel.Players.Any(p => p is Person))
                gameModel.Players[1] = new Person();
            gameInteractive = new InteractiveGame()
            {
                GameModel = gameModel
            };
            gameInteractive.StartGame();
            UpdateInteractiveStats();
        }
        void btnStartTrain_Clicked(object sender, RoutedEventArgs args)
        {
            ClearVisualStats();
            UpdatePlayBoardSize();
            gameModel.Players[0] = gameModel.Get<Agent>();
            gameModel.Players[1] = inputViewModel.TrainOpponent;
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
        void btnResetNet_Clicked(object sender, RoutedEventArgs args)
        {
            ResetNet();
        }
        void timer_Tick(object sender, EventArgs args)
        {
            ClearGridContent();
            gameModel.PlayBoard = new PlayBoard(gameModel.PlayBoard.Size);
            GameState gameState = game.Play();
            gameModel.SwitchPlayers();
            UpdateVisualStats(gameState);
        }
        async void btnGrid_Click(object sender, RoutedEventArgs args)
        {
            Button btn = sender as Button;
            if (btn.Content != null && btn.Content.ToString() != "") return;

            (int X, int Y) move = (Grid.GetRow(btn), Grid.GetColumn(btn));
            btn.Content = gameModel.CurrentTurn;
            gameModel.PlayBoard.PlaceMove(move, gameModel.CurrentTurn);
            lbStatusBar.Content = "Ваш ход сделан. Анализ игрового поля...";
            AllowGridClick(false);
            await Task.Delay(TimeSpan.FromSeconds(1.5));
            gameModel.Get<Person>().StatusBarLabel = "Сеть сделала свой выбор. ";
            gameInteractive.EndMove();
            UpdateInteractiveStats();
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
            foreach (Button btn in gridPlayBoard.Children)
                btn.IsHitTestVisible = isGame;
        }
        void PlaceMoveOnGrid((int X, int Y) move, PlayerTurn playerTurn)
        {
            foreach (Button btn in gridPlayBoard.Children)
                if (Grid.GetRow(btn) == move.X && Grid.GetColumn(btn) == move.Y)
                    btn.Content = playerTurn;
            
        }
        void ClearGridContent()
        {
            foreach (Button btn in gridPlayBoard.Children)
                btn.Content = "";
        }
        void UpdatePlayBoardSize()
        {
            if (gameModel.PlayBoard.Size != inputViewModel.BoardSize)
            {
                gameModel.PlayBoard.Size = inputViewModel.BoardSize;
                ResetNet();
                InitializeGrid();
            }
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

            if (trainStats.RoundsCount % inputViewModel.TrainStep == 0)
            {
                if (!gameModel.Get<Agent>().InTraining)
                {
                    StatsViewModel stats = new StatsViewModel(trainStats, inputViewModel.TrainStep);
                    lvResultTable.Items.Add(stats);
                }
                gameModel.Get<Agent>().ToggleTraining();
                graphViewModel.Add(trainStats, inputViewModel.TrainStep);
                pltResultGraph.InvalidatePlot(true);
                trainStats.ClearResults();
            }
            if (trainStats.RoundsCount == inputViewModel.RoundsAmount)
            {
                timer.Stop();
                lbStatusBar.Content = "Обучение завершено. ";
            }
        }
        void ClearVisualStats()
        {
            tbRoundsCount.Text = "";
            trainStats.Clear();
            graphViewModel.Clear();
            pltResultGraph.InvalidatePlot();
            lvResultTable.Items.Clear();
        }
        
        void UpdateInteractiveStats()
        {
            List<(int X, int Y)> playedMoves = gameModel.PlayBoard.PlayedMoves;
            List<int> playedValues = gameModel.PlayBoard.PlayedValues;
            if(gameModel.GameState != GameState.InGame)
            {
                if (gameModel.GetCurrentPlayer() is Agent)
                    PlaceMoveOnGrid(playedMoves.Last(), (PlayerTurn)playedValues.Last());
                
                gameModel.SwitchPlayers();
            }
            else if (gameModel.PlayBoard.PlayedMoves.Count > 0)
            {

                PlaceMoveOnGrid(playedMoves.Last(), (PlayerTurn)playedValues.Last());
                AllowGridClick(true);
            }
            lbStatusBar.Content = gameModel.Get<Person>().StatusBarLabel;
        }
        void ResetNet()
        {
            gameModel.Set<Agent>(new Agent(gameModel.PlayBoard.Size));
            lbStatusBar.Content = "Сеть обновлена. ";
        }
        
    }
}
