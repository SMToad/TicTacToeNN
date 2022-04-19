using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF.GameElements;
using TicTacToeWPF.Players;

namespace TicTacToeWPF.Models
{
    public class GameModel
    {
        public PlayBoard PlayBoard { get; set; }
        public List<Player> Players { get; set; }
        public PlayerTurn CurrentTurn { get; private set; } = PlayerTurn.X;
        
        public GameState GameState { get; set; }
        public void UpdateCurrentTurn() => CurrentTurn = (PlayerTurn)((int)CurrentTurn * -1);
        public Player GetCurrentPlayer()
        {
            return (CurrentTurn == PlayerTurn.X)? Players.First(): Players.Last();
        }
        public T Get<T>() where T : Player
        {
            return ((Players.First() is T) ? Players.First() : Players.Last()) as T;
        }
        public void Set<T>(T player) where T : Player
        {
            if (Players.First() is T) Players[0] = player;
            else Players[1] = player;
        }
        public void SwitchPlayers()
        {
            Players.Reverse();
            CurrentTurn = PlayerTurn.X;
        }
        
    }
}
