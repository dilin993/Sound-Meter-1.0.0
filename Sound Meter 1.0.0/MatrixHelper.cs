using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sound_Meter_1._0._0
{
    class MatrixHelper
    {

        public static bool isCompatible(int[,] M1, int[,] M2)
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

        public static int [,] add(int[,] M1, int[,] M2)
        {
            if (!isCompatible(M1, M2))
                return null;
            int[,] ans = new int[M1.GetLength(0), M1.GetLength(1)];
            for(int i=0;i<M1.GetLength(0);i++)
            {
                for(int j=0;j<M2.GetLength(1);j++)
                {
                    ans[i,j] = M1[i, j] + M2[i, j];
                }
            }
            return ans;
        }

        public static int[,] substract(int[,] M1, int[,] M2)
        {
            if (!isCompatible(M1, M2))
                return null;
            int[,] ans = new int[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    ans[i, j] = M1[i, j] - M2[i, j];
                }
            }
            return ans;
        }

        public static int[,] abs(int[,] M1)
        {
            int[,] ans = new int[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    ans[i, j] = Math.Abs(ans[i,j]);
                }
            }
            return ans;
        }

        public static int[,] threshold(int[,] M1, int th=0)
        {
            int[,] ans = new int[M1.GetLength(0), M1.GetLength(1)];
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

        public static int[,] init_mat(int m, int n, int val)
        {
            int[,] ans = new int[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    ans[i, j] = val;
                }
            }
            return ans;
        }

        public static int[,] zeros(int m,int n)
        {
            return init_mat(m, n, 0);
        }

        public static int[,] div(int[,] M1, double d)
        {
            int[,] ans = new int[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    ans[i, j] = (int)Math.Round(M1[i, j] / d);
                }
            }
            return ans;
        }

        public static int[] min(int[,] M1)
        {
            int[] curMin = { int.MaxValue, 0, 0 };
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

        public static int[] max(int[,] M1)
        {
            int[] curMax = { int.MinValue, 0, 0 };
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
