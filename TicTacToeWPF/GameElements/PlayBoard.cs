using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TicTacToeWPF.Helpers;

namespace TicTacToeWPF.GameElements
{
    public class PlayBoard
    {
        public List<(int X, int Y)> PlayedMoves { get; set; }
        public List<int> PlayedValues { get; set; }
   
        public int Size { get; set; }
        public PlayBoard(int size = 3)
        {
            Size = size;
            PlayedMoves = new List<(int X, int Y)>(size * size);
            PlayedValues = new List<int>(size * size);
        }
        public void InvertValues()
        {
            for(int i=0; i < PlayedValues.Count; i++)
                PlayedValues[i] *= -1;
        }
        public int[,] GetBoard()
        {
            int[,] board = new int[Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    int index = PlayedMoves.IndexOf((i, j));
                    if (index != -1)
                        board[i, j] = PlayedValues[index];
                    else board[i, j] = 0;
                }
            return board;
        }
        public int[] GetBoardArray()
        {
            int[] board = new int[Size * Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    int index = PlayedMoves.IndexOf((i, j));
                    if (index != -1)
                        board[i * Size + j] = PlayedValues[index];
                    else board[i * Size + j] = 0;
                }
            return board;
        }
        public List<(int X, int Y)> AvailableMoves() 
        {
            List<(int X, int Y)> moves = new List<(int X, int Y)> ();
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (PlayedMoves.IndexOf((i, j)) == -1)
                        moves.Add((i, j));
            return moves;
        }

        public List<int> BlockedMovesIndexes()
        {
            int[] board = GetBoardArray();
            return board.Where(x => x == 0).Select(x => Array.IndexOf(board, x)).ToList();
        }

        public void PlaceMove((int X, int Y) move, PlayerTurn playerTurn)
        {
            PlayedMoves.Add(move);
            PlayedValues.Add((int)playerTurn);
        }
    }
}

