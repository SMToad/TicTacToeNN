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
                Players = (new Agent(), null)
            };

            appViewModel = new AppViewModel()
            {
                TrainRounds = 50
            };

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(0.001);

            cbOpponent.ItemsSource = new List<string>() { "Человек", "Рандом", "Подряд", "Сама с собой" };

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
        }
        
        void UpdateVisualStats(GameState gameState)
        {
            trainStats.Increment(gameState);
            tbRoundsCount.Text = "Раунд " + trainStats.RoundsCount;

            int[,] board = gameModel.PlayBoard.Board;
            foreach (Button btn in gridPlayBoard.Children)
            {
                btn.Content = board[Grid.GetRow(btn), Grid.GetColumn(btn)] == 1 ? "X" : "O";
            }

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
            lbStatusBar.Content = "Ход сделан. Ожидание хода сети. ";
            gameInteractive.EndMove(move, ref btn);
        }

        void cbOpponent_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            switch (cbOpponent.SelectedIndex)
            {
                case 0:
                    gameModel.Players = (gameModel.Players.X, new Person(lbStatusBar));
                    break;
                case 1:
                    gameModel.Players = (gameModel.Players.X, new RandomPlayer());
                    break;
                case 2:
                    gameModel.Players = (gameModel.Players.X, new BoringPlayer());
                    break;
                case 3:
                    gameModel.Players = (gameModel.Players.X, gameModel.Players.X);
                    break;
            }
        }
        void timer_Tick(object sender, EventArgs args)
        {
            ClearGridContent();
            gameModel.PlayBoard = new PlayBoard(gameModel.PlayBoard.Size);
            GameState gameState = game.PlayRound();
            gameModel.Players = (gameModel.Players.O, gameModel.Players.X);
            UpdateVisualStats(gameState);
        }
        void btnResetNet_Clicked(object sender, RoutedEventArgs args)
        {
            ResetNet(GetPlayBoardSizeInput());
        }
        void ResetNet(int boardSize)
        {
            gameModel.PlayBoard.Size = boardSize;
            gameModel.SetAgent(new Agent(boardSize));

            InitializeGrid();
            trainStats.Clear();

            tbRoundsCount.Text = "Раунд ";
            lbStatusBar.Content = "Сеть обновлена. ";

            ClearVisualStats();
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
        int GetTotalRoundsInput()
        {
            if (tbTotalRounds.Text == "")
            {
                lbStatusBar.Content = "Задайте количество тренировочных игр!";
                return appViewModel.TotalRounds;
            }
            return Convert.ToInt32(tbTotalRounds.Text) * 2;
        }
        void UpdateInputFields()
        {
            int boardSize = GetPlayBoardSizeInput();
            if (gameModel.PlayBoard.Size != boardSize)
            {
                ResetNet(boardSize);
            }
            appViewModel.TotalRounds = GetTotalRoundsInput();
        }
        void StartInteractiveGame()
        {
            foreach (Button btn in gridPlayBoard.Children)
                btn.IsHitTestVisible = true;
            gameModel.PlayBoard = new PlayBoard(gameModel.PlayBoard.Size);
            gameInteractive = new InteractiveGame()
            {
                GameModel = gameModel,
                GridPlayBoard = gridPlayBoard
            };
            lbStatusBar.Content = "Игра против сети. ";
            gameInteractive.StartGame();
        }
        void btnStart_Clicked(object sender, RoutedEventArgs args)
        {
            ClearVisualStats();
            UpdateInputFields();
            InitializeGrid();
            trainStats.Clear();
            if (cbOpponent.SelectedIndex == 0)
            {
                StartInteractiveGame();
            }
            else
            {
                game = new Game()
                {
                    GameModel = gameModel
                };
                lbStatusBar.Content = "Обучение сети в процессе. ";
                timer.Start();
            }
        }
        void btnStop_Clicked(object sender, RoutedEventArgs args)
        {
            if (cbOpponent.SelectedIndex == 0)
            {
                lbStatusBar.Content = "Игра завершена. ";
                foreach (Button btn in gridPlayBoard.Children)
                    btn.IsHitTestVisible = false;
            }
            else
            {
                timer.Stop();
                lbStatusBar.Content = "Обучение завершено. ";
            }
        }
    }
}
