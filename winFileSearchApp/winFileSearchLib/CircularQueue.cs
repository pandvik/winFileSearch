using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winFileSearchLib
{
    public class CircularQueue<T>
    {
        Queue<T> q;
        int size;
        public CircularQueue(int size)
        {
            this.q = new Queue<T>(size);
            this.size = size;
        }

        public void push(T x)
        {
            if (q.Count >= size)
            {
                q.Dequeue();
            }
            q.Enqueue(x);
        }
        public bool cmp(IList<T> x)
        {
            if (x == null
                || x.Count != this.q.Count)
            {
                return false;
            }
            var maxLen = x.Count > this.q.Count ? x.Count : this.q.Count;
            var qList = q.GetEnumerator();
            var xList = x.GetEnumerator();
            for (int i = 0; i < maxLen; i++)
            {
                // if no more elements
                if (qList.MoveNext() == false
                || xList.MoveNext() == false)
                {
                    return false;
                }

                if (!qList.Current.Equals(xList.Current))
                    return false;
            }
            return true;
        }

    }
}
