using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Sound_Meter_1._0._0
{
    class Calculations
    {
        public static double[,] calculate_map(double[] power, int n)
        {
            double[,] M0 = MatrixHelper.zeros(n, n);
            double[,] M1 = MatrixHelper.zeros(n, n);
            double[,] M2 = MatrixHelper.zeros(n, n);
            double[,] M3 = MatrixHelper.zeros(n, n);
            double[,] Map = MatrixHelper.zeros(n, n);

            double r;

            for(int i=0;i<n;i++)
            {
                for(int j=0;j<n;j++)
                {
                    r = Math.Sqrt(Math.Pow(1.0 * i / n, 2) 
                        + Math.Pow(1.0 * j / n, 2));
                    r =  Math.Pow(1.0 + r, 2); // (1+r)^2
                    M0[i, j] = r * power[0];
                    M1[n-1-i, j] = r * power[1];
                    M2[i, n-1-j] = r * power[2];
                    M3[n-1-i, n-1-j] = r * power[3];
                }
            }

            double[,] M4 = MatrixHelper.threshold(MatrixHelper.substract(M0, M3));
            double[,] M5 = MatrixHelper.threshold(MatrixHelper.substract(M3, M0));
            double[,] M6 =MatrixHelper.threshold(MatrixHelper.substract(M1, M2));
            double[,] M7 = MatrixHelper.threshold(MatrixHelper.substract(M2, M1));

            Map = MatrixHelper.add(M4, M5);
            Map = MatrixHelper.add(Map, M6);
            Map = MatrixHelper.add(Map, M7);
            Map = MatrixHelper.div(Map, 4.0);

            double[] Max = MatrixHelper.max(Map);
            double[] Min = MatrixHelper.min(Map);
            double Mx = Max[0] - Min[0];
            int di = (int)Min[1];
            int dj = (int)Min[2];

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

        public static double[] calculate_fft(int[] x)
        {
            alglib.complex[] f;
            var d = x.Select(y => (double)y).ToArray();
            alglib.fftr1d(d, out f);
            double[] fft = new double[f.Length];
            for(int i=0;i<f.Length;i++)
            {
                fft[i] = Math.Sqrt(Math.Pow(f[i].x, 2) + (Math.Pow(f[i].y, 2)));
            }
            return fft;
        }

        public static int max_idx(double[] x)
        {
            double m = double.MinValue;
            int idx = 0;
            for(int i=0;i<x.Length;i++)
            {
                if(m<x[i])
                {
                    m = x[i];
                    idx = i;
                }
            }
            return idx;
        }

        public static double[] calculate_peak_power(double[] fft1, double[] fft2, double[] fft3, double[] fft4)
        {
            int[] maxI = { max_idx(fft1), max_idx(fft2), max_idx(fft3), max_idx(fft4) };
            double[] max = { fft1[maxI[0]], fft2[maxI[1]], fft3[maxI[2]], fft4[maxI[3]] };
            int maxJ = max_idx(max);
            int i = maxI[maxJ];
            double[] ans = { fft1[i], fft2[i], fft3[i], fft4[i] };
            return ans;
        }
    }
}
