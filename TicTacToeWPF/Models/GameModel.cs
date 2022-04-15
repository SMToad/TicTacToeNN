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
        public (Player X, Player O) Players { get; set; }
        public PlayerTurn CurrentTurn { get; set; }
        public GameState GameState { get; set; }
        public Agent GetAgent()
        {
            return ((Players.X is Agent) ? Players.X : Players.O) as Agent;
        }
        public void SetAgent(Agent agent)
        {
            if (Players.X is Agent)
                Players = (agent, Players.O);
            else Players = (Players.X, agent);
        }
    }
}
