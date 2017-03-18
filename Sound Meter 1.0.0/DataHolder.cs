using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sound_Meter_1._0._0
{
    class DataHolder
    {
        Queue<Int16> data_ch0;
        Queue<Int16> data_ch1;
        Queue<Int16> data_ch2;
        Queue<Int16> data_ch3;

        int windowSize = 300;
        const int percentOverhead = 10;
        const int percentOverlap = 0;

        public int WindowsSize
        {
            get { return windowSize; }
        }

        public int Overhead
        {
            get { return windowSize * percentOverhead / 100; }
        }

        public int Overlap
        {
            get { return windowSize * percentOverlap / 100; }
        }

        public DataHolder(int windowSize)
        {
            this.windowSize = windowSize;
            int capacity = WindowsSize + Overhead;
            data_ch0 = new Queue<Int16>(capacity);
            data_ch1 = new Queue<Int16>(capacity);
            data_ch2 = new Queue<Int16>(capacity);
            data_ch3 = new Queue<Int16>(capacity);
        }
        
        public void enqueue(Int16[] data)
        {
            lock (this)
            {
                data_ch0.Enqueue(data[0]);
                data_ch1.Enqueue(data[1]);
                data_ch2.Enqueue(data[2]);
                data_ch3.Enqueue(data[3]);
            }
        }

        public bool isWindowsFull()
        {
            return data_ch0.Count >= WindowsSize;
        }

        private Int16[] read_top(Queue<Int16> q)
        {
            if(q.Count>=WindowsSize)
            {
                Int16[] top =  q.Take(WindowsSize).ToArray();
                // discard non overlap data out of queue
                int discardCount = WindowsSize - Overlap;
                for(int i=0;i<discardCount;i++)
                {
                    q.Dequeue();
                }
                return top;
            }
            return null;
        }

        public Int16[] read_ch0()
        {
            lock(this)
            {
                return read_top(data_ch0);
            }
        }

        public Int16[] read_ch1()
        {
            lock (this)
            {
                return read_top(data_ch1);
            }
        }

        public Int16[] read_ch2()
        {
            lock (this)
            {
                return read_top(data_ch2);
            }
        }

        public Int16[] read_ch3()
        {
            lock (this)
            {
                return read_top(data_ch3);
            }
        }

        public void clear()
        {
            lock (this)
            {
                data_ch0.Clear();
                data_ch1.Clear();
                data_ch2.Clear();
                data_ch3.Clear();
            }
        }

    }
}
