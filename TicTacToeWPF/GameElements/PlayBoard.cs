using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TicTacToeWPF.Helpers;

namespace TicTacToeWPF.GameElements
{
    public class PlayBoard
    {
        public int[,] Board { get; private set; }
   
        public int Size { get; set; }
        public PlayBoard(int size = 3)
        {
            Size = size;
            Board = new int[Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Board[i, j] = 0;
        }
        public PlayBoard(int[,] values)
        {
            Size = values.GetLength(0);
            Board = new int[Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Board[i, j] = values[i,j];
        }
        public void Invert()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Board[i, j] *= -1;
        }
        public List<(int X, int Y)> GetMoves(bool isAvailable)
        {
            List<(int X, int Y)> res = new List<(int, int)>();
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (!(Board[i, j] == 0 ^ isAvailable)) res.Add((i, j));
            return res;
        }
        public List<(int X, int Y)> AvailableMoves() =>  GetMoves(true);
        
        public List<int> BlockedMoves() => GetMoves(false).Flatten(Size);
        
        public void MakeMove((int X, int Y) move, int value)
        {
            Board[move.X, move.Y] = value;
        }
    }
}

