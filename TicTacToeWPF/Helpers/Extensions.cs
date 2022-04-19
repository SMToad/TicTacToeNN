using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeWPF.Helpers
{
    public static class Extensions
    {
        public static double[] ToDouble(this int[] array)
        {
            return array.Select(x => (double)x).ToArray();
        }
        public static double ToPercent(this int currAmount, int totalAmount)
        {
            return Math.Round((double)currAmount / totalAmount * 100, 2);
        }
    }
}
