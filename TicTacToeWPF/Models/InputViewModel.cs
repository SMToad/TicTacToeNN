using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF.Players;
using TicTacToeWPF.GameElements;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Data;

namespace TicTacToeWPF.Models
{
    public class Opponent
    {
        public string Type { get; set; }
        public Opponent(string type) => Type = type;
        public override string ToString() => Type;
    }
    public class InputViewModel: INotifyPropertyChanged
    {
        public InputViewModel()
        {
            IList<Opponent> list = new List<Opponent>();
            list.Add(new Opponent("Рандом"));
            list.Add(new Opponent("Подряд"));
            list.Add(new Opponent("Сеть"));
            _opponents = new CollectionView(list);
        }

        private readonly CollectionView _opponents;

        public CollectionView Opponents
        {
            get { return _opponents; }

        }
        private int _boardSize = 3;
        public int BoardSize
        {
            get { return _boardSize; }
            set
            {
                if (_boardSize == value) return;
                _boardSize = Math.Max(3, Math.Min(value, 5)); ;
                OnPropertyChanged("BoardSize");
            }
        }
        private string _opponent = "Рандом";
        public string Opponent
        {
            get { return _opponent; }
            set
            {
                if (_opponent == value) return;
                _opponent = value;
                OnPropertyChanged("Opponent");
            }
        }
        public Player TrainOpponent
        {
            get
            {
                switch(Opponent)
                {
                    case "Рандом":
                        return new RandomPlayer();
                    case "Подряд":
                        return new BoringPlayer();
                    case "Сама сеть":
                        return new Agent();
                    default: return null;
                }
            }
        }

        private int _roundsAmount = 100;
        public int RoundsAmount
        {
            get { return _roundsAmount; }
            set
            {
                if (_roundsAmount == value) return;
                _roundsAmount = Math.Max(5, Math.Min(value, 50000));
                OnPropertyChanged("RoundsAmount");
            }
        }
        public int TrainStep { get => Math.Max(RoundsAmount, 1); }
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
