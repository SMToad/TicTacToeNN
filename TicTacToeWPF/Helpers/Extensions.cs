using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeWPF.Helpers
{
    public static class Extensions
    {
        public static int[] Flatten(this int[,] array) => array.Cast<int>().ToArray();
        public static double[] FlattenDouble(this int[,] array) => array.Cast<double>().ToArray();
        public static List<int> Flatten(this List<(int,int)> list, int size) => 
            list.Select(x => x.Item1 * size + x.Item2).ToList();
        public static double ToPercent(this int currAmount, int totalAmount)
        {
            return Math.Round((double)currAmount / totalAmount * 100, 2);
        }
    }
}
