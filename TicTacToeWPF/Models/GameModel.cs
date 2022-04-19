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
        public PlayerTurn CurrentTurn
        {
            get
            {
                if (PlayBoard.PlayedValues.Count > 0)
                    return (PlayerTurn)(PlayBoard.PlayedValues.Last() * -1);
                else return PlayerTurn.X;
            }
        }
        public GameState GameState { get; set; }
        public void PlaceMoveOnBoard(Player player)
        {
            PlayBoard.PlaceMove(player.LastMove, CurrentTurn);
        }
        public Agent GetAgent()
        {
            return ((Players.First() is Agent) ? Players.First() : Players.Last()) as Agent;
        }
        public void SetAgent(Agent agent)
        {
            if (Players.First() is Agent) Players[0] = agent;
            else Players[1] = agent;
        }
        public void SwitchPlayers()
        {
            Players.Reverse();
        }
        public List<Player> CopyPlayers()
        {
            List<Player> copy = new List<Player>(2);
            Players.ForEach(p => copy.Add(p));
            return copy;
        }
    }
}
