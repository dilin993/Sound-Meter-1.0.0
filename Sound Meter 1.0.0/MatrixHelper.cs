using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sound_Meter_1._0._0
{
    class MatrixHelper
    {

        public static bool isCompatible(Int16[,] M1, Int16[,] M2) 
        {
            if (M1.Rank != M2.Rank)
                return false;
            for(int i=0;i<M1.Rank;i++)
            {
                if (M1.GetLength(i) != M2.GetLength(i))
                    return false;
            }
            return true;
        }

        public static Int16[,] add(Int16[,] M1, Int16[,] M2)
        {
            if (!isCompatible(M1, M2))
                return null;
            Int16[,] ans = new Int16[M1.GetLength(0), M1.GetLength(1)];
            for(int i=0;i<M1.GetLength(0);i++)
            {
                for(int j=0;j<M2.GetLength(1);j++)
                {
                    ans[i,j] = (Int16)(M1[i, j] + M2[i, j]);
                }
            }
            return ans;
        }

        public static Int16[,] substract(Int16[,] M1, Int16[,] M2)
        {
            if (!isCompatible(M1, M2))
                return null;
            Int16[,] ans = new Int16[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    ans[i, j] = (Int16)(M1[i, j] - M2[i, j]);
                }
            }
            return ans;
        }

        public static Int16[,] abs(Int16[,] M1)
        {
            Int16[,] ans = new Int16[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    ans[i, j] = (Int16)Math.Abs(ans[i,j]);
                }
            }
            return ans;
        }

        public static Int16[,] threshold(Int16[,] M1, Int16 th =0)
        {
            Int16[,] ans = new Int16[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    if (M1[i, j] < th)
                        ans[i, j] = 0;
                    else
                        ans[i, j] = M1[i, j];
                }
            }
            return ans;
        }

        public static Int16[,] init_mat(int m, int n, Int16 val)
        {
            Int16[,] ans = new Int16[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    ans[i, j] = val;
                }
            }
            return ans;
        }

        public static Int16[,] zeros(int m,int n)
        {
            return init_mat(m, n, 0);
        }

        public static Int16[,] div(Int16[,] M1, Int16 d)
        {
            Int16[,] ans = new Int16[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    ans[i, j] = (Int16)(M1[i, j] / (d*1.0));
                }
            }
            return ans;
        }

        public static int[] min(Int16[,] M1)
        {
            int[] curMin = { Int16.MaxValue, 0, 0 };
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    if (M1[i, j] < curMin[0])
                    {
                        curMin[0] = M1[i, j];
                        curMin[1] = i;
                        curMin[2] = j;
                    }
                }
            }
            return curMin;
        }

        public static int[] max(Int16[,] M1)
        {
            int[] curMax = { Int16.MinValue, 0, 0 };
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    if (M1[i, j] > curMax[0])
                    {
                        curMax[0] = M1[i, j];
                        curMax[1] = i;
                        curMax[2] = j;
                    }
                }
            }
            return curMax;
        }


    }
}
