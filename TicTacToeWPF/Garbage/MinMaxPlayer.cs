//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TicTacToeWPF
//{
//    class MinMaxPlayer:Player
//    {
//        public List<(double[,], Tuple<int,int>)> cashMoves;
//        public MinMaxPlayer()
//        {
//            cashMoves = new List<(double[,], Tuple<int, int>)>();
//        }
//        public override Tuple<int, int> Move(PlayBoard playBoard, bool isFirst)
//        {
//            var board = playBoard.GetBoard();
//            for (int i = 0; i < cashMoves.Count; i++)
//                if (board.Cast<double>().SequenceEqual(cashMoves[i].Item1.Cast<double>()))
//                    return cashMoves[i].Item2;
//            Tuple<int, int> bestMove = new Tuple<int, int>(-1, -1);
//            int bestScore = int.MinValue;
//            var avMoves = playBoard.AvailableMoves();
//            foreach(var el in avMoves)
//            {
//                PlayBoard tempBoard = new Board(board);
//                tempBoard.MakeMove(new Tuple<int, int>(el.Item1,el.Item2), isFirst ? -1 : 1);
//                int score = -Minimax(tempBoard, !isFirst, 0);
//                if(score>bestScore)
//                {
//                    bestScore = score;
//                    bestMove = new Tuple<int, int>(el.Item1, el.Item2);
//                }
//            }
//            cashMoves.Add((board, bestMove));
//            return bestMove;
//        }
//        protected virtual int Minimax(PlayBoard playBoard, bool isFirst, int depth)
//        {
//            const int alpha = -5;
//            const int beta = 5;
//            const int maxScore = alpha;
//            return AlphaBeta(playBoard, isFirst, depth, -beta, -maxScore);
//        }
//        private int AlphaBeta(PlayBoard playBoard, bool isFirst, int depth, int alpha, int beta)
//        {
//            Game.Result res = Game.CheckGameEnd(playBoard);
//            if (res != Game.Result.Unknown)
//                return -10 + depth;
//            var avMoves = playBoard.AvailableMoves();
//            if (avMoves.Count == 0)
//                return 0;
//            int maxScore = alpha;
//            foreach (var el in avMoves)
//            {
//                PlayBoard tempBoard = new Board(playBoard.GetBoard());
//                tempBoard.MakeMove(new Tuple<int, int>(el.Item1, el.Item2), isFirst ? -1 : 1);
//                int score = -AlphaBeta(tempBoard, !isFirst, depth + 1,-beta,-maxScore);
//                if (score > maxScore)
//                {
//                    maxScore = score;
//                    if (maxScore >= beta)
//                        break;
//                }
//            }
//            return maxScore;
//        }
//        /*protected virtual int Minimax(Board playBoard, bool isFirst, int depth)
//        {
//            Game.Result res = Game.check_game_end(playBoard);
//            if (res != Game.Result.Unknown)
//                return -10 + depth;
//            var avMoves = playBoard.available_moves();
//            if (avMoves.Count == 0) 
//                return 0;
//            int maxScore = int.MinValue;
//            foreach(var el in avMoves)
//            {
//                Board tempBoard = new Board(playBoard.GetBoard());
//                tempBoard.makeMove(new Tuple<int, int>(el.Item1, el.Item2), isFirst ? -1 : 1);
//                int score = -Minimax(tempBoard, !isFirst, depth+1);
//                if (score > maxScore)
//                {
//                    maxScore = score;
//                }
//            }
//            return maxScore;
//        }*/
//    }
//}
