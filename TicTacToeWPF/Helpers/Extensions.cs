using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToeWPF.GameElements;

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
        public static float ToFloat(this GameState gameState)
        {
            switch (gameState)
            {
                case GameState.XWin:
                    {
                        return 1f;
                    }
                case GameState.OWin:
                    {
                        return -1f;
                    }
                case GameState.Tie:
                    {
                        return 0.5f;
                    }
                default:
                    {
                        return 0f;
                    }
            }
        }
    }
}
