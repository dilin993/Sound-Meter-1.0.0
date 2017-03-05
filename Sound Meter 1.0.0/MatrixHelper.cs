using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sound_Meter_1._0._0
{
    class MatrixHelper
    {

        public static bool isCompatible(double[,] M1, double[,] M2)
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

        public static double [,] add(double[,] M1, double[,] M2)
        {
            if (!isCompatible(M1, M2))
                return null;
            double[,] ans = new double[M1.GetLength(0), M1.GetLength(1)];
            for(int i=0;i<M1.GetLength(0);i++)
            {
                for(int j=0;j<M2.GetLength(1);j++)
                {
                    ans[i,j] = M1[i, j] + M2[i, j];
                }
            }
            return ans;
        }

        public static double[,] substract(double[,] M1, double[,] M2)
        {
            if (!isCompatible(M1, M2))
                return null;
            double[,] ans = new double[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    ans[i, j] = M1[i, j] - M2[i, j];
                }
            }
            return ans;
        }

        public static double[,] abs(double[,] M1)
        {
            double[,] ans = new double[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    ans[i, j] = Math.Abs(ans[i,j]);
                }
            }
            return ans;
        }

        public static double[,] threshold(double[,] M1, double th=0)
        {
            double[,] ans = new double[M1.GetLength(0), M1.GetLength(1)];
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

        public static double[,] init_mat(int m, int n, double val)
        {
            double[,] ans = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    ans[i, j] = val;
                }
            }
            return ans;
        }

        public static double[,] zeros(int m,int n)
        {
            return init_mat(m, n, 0);
        }

        public static double[,] div(double[,] M1, double d)
        {
            double[,] ans = new double[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M1.GetLength(1); j++)
                {
                    ans[i, j] = M1[i, j] / d;
                }
            }
            return ans;
        }

        public static double[] min(double[,] M1)
        {
            double[] curMin = { double.MaxValue, 0, 0 };
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

        public static double[] max(double[,] M1)
        {
            double[] curMax = { double.MinValue, 0, 0 };
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
