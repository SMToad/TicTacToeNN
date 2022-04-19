using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF.Helpers;

namespace TicTacToeWPF
{
    public class MemoryBuffer
    {
        public int Size { get; set; }
        int MaxSize { get; set; }
        public List<MemoryEntry> History { get; set; }
        public MemoryBuffer(int size = 3000)
        {
            Size = 0;
            MaxSize = size;
            History = new List<MemoryEntry>();
        }
        public void Add(MemoryEntry memoryEntry)
        {
            if (MaxSize == Size)
            {
                History.RemoveAt(0);
            }
            else Size++;
            History.Add(memoryEntry);
            if (Size > 1 && memoryEntry.Reward == 0)
                History[Size - 2].NextState = memoryEntry.State;
        }
        public void Add(MemoryBuffer buffer)
        {
            for (int i = 0; i < buffer.Size; i++)
            {
                Add(buffer.History[i]);
            }
        }
        public MemoryBuffer GetSample(int size)
        {
            int minSize = Math.Min(Size, size);
            Random rand = new Random();
            MemoryBuffer sample = new MemoryBuffer(minSize);
            List<int> randIndexes = new List<int>();
            for (int i = 0; i < minSize; i++)
            {
                int j;
                do
                    j = rand.Next(0, Size);
                while (randIndexes.Contains(j));
                randIndexes.Add(j);
                sample.Add(History[j]);
            }
            return sample;
        }
        public void AddRewardData(float rewardValue)
        {
            for (int i = Size - 1; i > -1; i--)
                History[i].Reward = rewardValue / (Size - i);
        }
    }
}
