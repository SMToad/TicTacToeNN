//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace TicTacToeWPF
//{
//    class PerfectPlayer:Player
//    {
//        public List<(double[,], List<(int,int,int)>)> cashMoves;
//        Random rand = new Random();
//        public PerfectPlayer()
//        {
//            cashMoves = new List<(double[,], List<(int, int, int)>)>();
//        }
//        public override Tuple<int, int> Move(PlayBoard playBoard, bool isFirst=true)
//        {
//            var res = max(playBoard, !isFirst);
//            return new Tuple<int, int>(res.Item1, res.Item2);
//        }
//        public int isInCash(double[,] arr)
//        {
//            int ind = -1;
//            bool res = false;
//            for (int i = 0; i < cashMoves.Count&&!res; i++)
//            {
//                bool temp = true;
//                for (int j = 0; j < arr.GetLength(0) && temp; j++)
//                    for (int k = 0; k < arr.GetLength(1) && temp; k++)
//                        if (arr[j, k] != cashMoves[i].Item1[j, k]) temp = false;
//                res = temp;
//                ind = i;
//            }
//            return res == true ? ind : -1;
//        }
//        public Tuple<int, int, int> max(PlayBoard playboard, bool isFirst)
//        {
//            double[,] board = playboard.GetBoard();
//            for (int ind = 0; ind < cashMoves.Count; ind++)
//                if (board.Cast<double>().SequenceEqual(cashMoves[ind].Item1.Cast<double>()))
//                {
//                    int k = rand.Next(0, cashMoves[ind].Item2.Count);
//                    return new Tuple<int, int, int>( cashMoves[ind].Item2[k].Item1, cashMoves[ind].Item2[k].Item2, cashMoves[ind].Item2[k].Item3);
//                }
//            int max = 0;
//            Tuple<int, int> max_move = new Tuple<int, int>(-1, -1);
//            List<(int, int, int)> bestMoves = new List<(int, int, int)>();
//            bestMoves.Add((max_move.Item1, max_move.Item2, max));
//            Game.Result res = Game.CheckGameEnd(playboard);
//            if (res == Game.Result.XWins)
//            {
//                max = isFirst ? 1 : -1;
//                bestMoves[0] = (max_move.Item1,max_move.Item2, max);
//            }
//            else if (res == Game.Result.OWins)
//            {
//                max = isFirst ? -1 : 1;
//                bestMoves[0] = (max_move.Item1, max_move.Item2, max);
//            }
//            else
//            {
//                List<(int, int)> avMoves = playboard.AvailableMoves();
//                foreach (var el in avMoves)
//                {
//                    PlayBoard tempBoard = new Board(board);
//                    tempBoard.MakeMove(new Tuple<int, int>(el.Item1, el.Item2), isFirst ? 1 : -1);
//                    var tempRes = min(tempBoard, isFirst);
//                    if (tempRes.Item3 > max || (max_move.Item1 == -1 && max_move.Item2 == -1))
//                    {
//                        max = tempRes.Item3;
//                        max_move = new Tuple<int, int>(el.Item1, el.Item2);
//                        bestMoves[0] = (max_move.Item1, max_move.Item2, max);
//                    }
//                    else if (max == 1)
//                    {
//                        max_move = new Tuple<int, int>(el.Item1, el.Item2);
//                        bestMoves.Add((max_move.Item1, max_move.Item2, max));
//                    }
                    
//                }
//            }
//            cashMoves.Add((board, bestMoves));
//            int i = rand.Next(0, bestMoves.Count);
//            return new Tuple<int, int, int>(bestMoves[i].Item1, bestMoves[i].Item2, bestMoves[i].Item3);

//        }
//        public Tuple<int, int, int> min(PlayBoard playboard, bool isFirst)
//        {
//            double[,] board = playboard.GetBoard();

//            for (int ind = 0; ind < cashMoves.Count; ind++)
//                if (board.Cast<double>().SequenceEqual(cashMoves[ind].Item1.Cast<double>()))
//                {
//                    int k = rand.Next(0, cashMoves[ind].Item2.Count);
//                    return new Tuple<int, int, int>(cashMoves[ind].Item2[k].Item1, cashMoves[ind].Item2[k].Item2, cashMoves[ind].Item2[k].Item3);
//                }
//            int min = 0;
//            Tuple<int, int> min_move = new Tuple<int, int>(-1, -1);
//            List<(int, int, int)> bestMoves = new List<(int, int, int)>();
//            bestMoves.Add((min_move.Item1, min_move.Item2, min));
//            Game.Result res = Game.CheckGameEnd(playboard);
//            if (res == Game.Result.XWins)
//            {
//                min = isFirst ? 1 : -1;
//                bestMoves[0] = (min_move.Item1, min_move.Item2, min);
//            }
//            else if (res == Game.Result.OWins)
//            {
//                min = isFirst ? -1 : 1;
//                bestMoves[0] = (min_move.Item1, min_move.Item2, min);
//            }
//            else
//            {
//                List<(int, int)> avMoves = playboard.AvailableMoves();
//                foreach (var el in avMoves)
//                {
//                    PlayBoard tempBoard = new Board(board);
//                    tempBoard.MakeMove(new Tuple<int, int>(el.Item1, el.Item2), isFirst ? -1 : 1);
//                    var tempRes = max(tempBoard, isFirst);
//                    if (tempRes.Item3 < min || (min_move.Item1 == -1 && min_move.Item2 == -1))
//                    {
//                        min = tempRes.Item3;
//                        min_move = new Tuple<int, int>(el.Item1, el.Item2);
//                        bestMoves[0] = (min_move.Item1, min_move.Item2, min);
//                    }
//                    else if (min == -1)
//                    {
//                        min_move = new Tuple<int, int>(el.Item1, el.Item2);
//                        bestMoves.Add((min_move.Item1, min_move.Item2, min));
//                    }

//                }
//            }
//            cashMoves.Add((board, bestMoves));
//            int i = rand.Next(0, bestMoves.Count);
//            return new Tuple<int, int, int>(bestMoves[i].Item1, bestMoves[i].Item2, bestMoves[i].Item3);

//        }
//    }
//}
