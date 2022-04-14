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
        AppViewModel appViewModel;
        GraphViewModel graphViewModel;
        TrainStats trainStats;
        (Player X, Player O) players;


        public MainWindow()
        {
            InitializeComponent();
            appViewModel = new AppViewModel()
            {
                PlayBoard = new PlayBoard(),
                Agent = new Agent(),
                Timer = new DispatcherTimer(),
                TrainRounds = 50
            };
            players.X = appViewModel.Agent;
            appViewModel.Timer.Tick += new EventHandler(Timer_Tick);
            appViewModel.Timer.Interval = TimeSpan.FromSeconds(0.001);
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

            int[,] board = appViewModel.PlayBoard.Board;
            foreach (Button btn in gridPlayBoard.Children)
            {
                btn.Content = board[Grid.GetRow(btn), Grid.GetColumn(btn)] == 1 ? "X" : "O";
            }

            if (trainStats.RoundsCount % appViewModel.TrainRounds == 0)
            {
                if (!appViewModel.Agent.InTraining)
                {
                    StatsViewModel stats = new StatsViewModel(trainStats, appViewModel.TrainRounds);
                    lvResultTable.Items.Add(stats);
                }
                appViewModel.Agent.ToggleTraining();
                graphViewModel.Add(trainStats, appViewModel.TrainRounds);
                pltResultGraph.InvalidatePlot(true);
                trainStats.ClearResults();
            }
            if (trainStats.RoundsCount == appViewModel.TotalRounds)
            {
                appViewModel.Timer.Stop();
                lbStatusBar.Content = "Обучение завершено";
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
            int size = appViewModel.PlayBoard.Size;
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
        
        /*
         * void FinishRound(GameState gameState)
        {
            if (gameState == Game.Result.XWins)
            {
                agent.Reward(isNetFirst ? 1 : -1);
                lbStatusBar.Content = "Крестики победили.";
            }
            else if (gameState == Game.Result.OWins)
            {
                agent.Reward(isNetFirst ? -1 : 1);
                lbStatusBar.Content = "Нолики победили.";
            }
            else
            {
                agent.Reward(0.5);
                lbStatusBar.Content = "Ничья.";
            }
            UpdateVisualStats(gameState);
            isNetFirst = !isNetFirst;
            agent.NewGame();
            playBoard = new Board(size);
            ClearGridContent();
            if (isNetFirst) MoveNN();
            else lbStatusBar.Content = lbStatusBar.Content.ToString() + " Ваш ход";
        }*/
        void btnGrid_Click(object sender, RoutedEventArgs args)
        {
            Button btn = sender as Button;
            if (btn.Content != null && btn.Content.ToString() != "") return;
            (int X, int Y) move = (Grid.GetRow(btn), Grid.GetColumn(btn));
            InteractiveGame.EndMove(appViewModel.PlayBoard, move, ref btn, appViewModel.Agent);
        }

        void cbOpponent_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            switch (cbOpponent.SelectedIndex)
            {
                case 1:
                    appViewModel.Opponent = new RandomPlayer();
                    break;
                case 2:
                    appViewModel.Opponent = new BoringPlayer();
                    break;
                case 3:
                    appViewModel.Opponent = appViewModel.Agent;
                    break;
            }
            players.O = appViewModel.Opponent;
        }
        void Timer_Tick(object sender, EventArgs args)
        {
            ClearGridContent();
            appViewModel.PlayBoard = new PlayBoard(appViewModel.PlayBoard.Size);
            GameState gameState = Game.Play(appViewModel.PlayBoard, players);
            players = (players.O, players.X);
            UpdateVisualStats(gameState);
        }
        void btnResetNet_Clicked(object sender, RoutedEventArgs args)
        {
            ResetNet(GetPlayBoardSizeInput());
        }
        void ResetNet(int boardSize)
        {
            appViewModel.PlayBoard.Size = boardSize;
            appViewModel.Agent = new Agent(boardSize);

            InitializeGrid();
            trainStats.Clear();

            tbRoundsCount.Text = "Раунд ";
            lbStatusBar.Content = "Сеть обновлена";

            ClearVisualStats();
        }
        int GetPlayBoardSizeInput()
        {
            if (tbPlayBoardSize.Text == "")
            {
                lbStatusBar.Content = "Задайте размер поля!";
                return appViewModel.PlayBoard.Size;
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
            if (appViewModel.PlayBoard.Size != boardSize)
            {
                ResetNet(boardSize);
            }
            appViewModel.TotalRounds = GetTotalRoundsInput();
        }
        void StartInteractiveGame()
        {
            ClearGridContent();
            foreach (Button btn in gridPlayBoard.Children)
                btn.IsHitTestVisible = true;
            lbStatusBar.Content = "Игра против сети. Ваш ход";
            appViewModel.PlayBoard = new PlayBoard(appViewModel.PlayBoard.Size);
            InteractiveGame.GridPlayBoard = gridPlayBoard;
            InteractiveGame.StartGame(appViewModel.PlayBoard, players);
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
                lbStatusBar.Content = "Обучение сети в процессе";
                appViewModel.Timer.Start();
            }
        }
        void btnStop_Clicked(object sender, RoutedEventArgs args)
        {
            if (cbOpponent.SelectedIndex == 0)
            {
                lbStatusBar.Content = "Игра завершена";
                foreach (Button btn in gridPlayBoard.Children)
                    btn.IsHitTestVisible = false;
            }
            else
            {
                appViewModel.Timer.Stop();
                lbStatusBar.Content = "Обучение завершено";
            }
        }
    }
}
