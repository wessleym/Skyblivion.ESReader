using System.Collections.Generic;

namespace Skyblivion.ESReader.QueueExtensions
{
    public static class QueueExtensions
    {
        public static IEnumerable<T> Dequeue<T>(this Queue<T> queue, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return queue.Dequeue();
            }
        }
    }
}