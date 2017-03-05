using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sound_Meter_1._0._0
{
    class Calculations
    {
        public int[,] calculate_map(int[] power, int n)
        {
            int[,] M0 = MatrixHelper.zeros(n, n);
            int[,] M1 = MatrixHelper.zeros(n, n);
            int[,] M2 = MatrixHelper.zeros(n, n);
            int[,] M3 = MatrixHelper.zeros(n, n);
            int[,] Map = MatrixHelper.zeros(n, n);

            double r;

            for(int i=0;i<n;i++)
            {
                for(int j=0;j<n;j++)
                {
                    r = Math.Sqrt(Math.Pow(1.0 * i / n, 2) 
                        + Math.Pow(1.0 * j / n, 2));
                    r =  Math.Pow(1.0 + r, 2); // (1+r)^2
                    M0[i, j] = (int)Math.Round(r * power[0]);
                    M1[n-1-i, j] = (int)Math.Round(r * power[1]);
                    M2[i, n-1-j] = (int)Math.Round(r * power[2]);
                    M3[n-1-i, n-1-j] = (int)Math.Round(r * power[3]);
                }
            }

            int[,] M4 = MatrixHelper.abs(MatrixHelper.threshold(MatrixHelper.substract(M0, M3)));
            int[,] M5 = MatrixHelper.abs(MatrixHelper.threshold(MatrixHelper.substract(M3, M0)));
            int[,] M6 = MatrixHelper.abs(MatrixHelper.threshold(MatrixHelper.substract(M1, M2)));
            int[,] M7 = MatrixHelper.abs(MatrixHelper.threshold(MatrixHelper.substract(M2, M1)));

            Map = MatrixHelper.add(M4, M5);
            Map = MatrixHelper.add(Map, M6);
            Map = MatrixHelper.add(Map, M7);
            Map = MatrixHelper.div(Map, 4.0);

            int[] Max = MatrixHelper.max(Map);
            int[] Min = MatrixHelper.min(Map);
            int Mx = Max[0] - Min[0];
            int di = Min[1];
            int dj = Min[2];

            for(int i=0;i<n;i++)
            {
                for(int j=0;j<n;j++)
                {
                    r = Math.Sqrt(Math.Pow(1.0 * (i-di) / n, 2)
                       + Math.Pow(1.0 * (j-dj) / n, 2));
                    Map[i, j] = (int)Math.Round(Mx / Math.Pow(1.0 + r, 2));
                }
            }
            return Map;
        }
    }
}
