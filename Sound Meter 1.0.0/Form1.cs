using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace Sound_Meter_1._0._0
{
    public partial class Form1 : Form
    {

        private CommunicationHandler chandler;
        private string port = "COM8";
        private int baudrate = 9600;
        private DataHolder datah;
        private const int WINDOW_SIZE = 1;
        OxyPlot.WindowsForms.PlotView plot1;
        private Bitmap bitmap;

        delegate void SetTextCallback(string text);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnFreeze.Enabled = false;
            datah = new DataHolder(WINDOW_SIZE);
            bitmap = new Bitmap(1000, 1000);
            // Add oxyplot components
            //plot1 = new OxyPlot.WindowsForms.PlotView();
            //this.SuspendLayout();
            // 
            // plot1
            // 
            //plot1.Dock = System.Windows.Forms.DockStyle.Fill;
            //plot1.Name = "plot1";
            //plot1.PanCursor = System.Windows.Forms.Cursors.Hand;
            //plot1.Text = "plot1";
            //plot1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            //plot1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            //plot1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            //pnlMap.Controls.Add(plot1);

            //Int16[] power = { 20,20,15,15};
            //draw_soundmap(power);
        }

        private void set_com_port(string port, int baudrate)
        {
            this.port = port;
            this.baudrate = baudrate;
            btnStart.Enabled = true;
            btnFreeze.Enabled = true;
        }

        private void status_update(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(status_update);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtStatus.AppendText(DateTime.Now.ToString("hh:mm:ss tt") + ">>> " + text +"\r\n");
            }
        }

       


        private void btnSettings_Click(object sender, EventArgs e)
        {
            frmSettings frmSet = new frmSettings();
            frmSet.ShowDialog();
            set_com_port(frmSet.PortName, frmSet.Baudrate);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(btnStart.Text.ToLower()=="start")
            {
                chandler = new CommunicationHandler(port, baudrate);
                try
                {
                    chandler.received += OnDataCollected;
                    txtStatus.Text = ""; // clear status
                    datah.clear();
                    chandler.start();
                    btnStart.Text = "Stop";
                    //timer1.Enabled = true;
                    //timer1.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                chandler.stop();
                btnStart.Text = "Start";
                //timer1.Stop();
                //timer1.Enabled = false;
            }
            
        }

        private void OnDataCollected(object sender, CommunicationHandlerEventArgs e)
        {

            string status =   "CH0: " + e.GetCH0 + ", " +
                              "CH1: " + e.GetCH1 + ", " +
                              "CH2: " + e.GetCH2 + ", " +
                              "CH3: " + e.GetCH3;
            status_update(status);
            Int16[] power = { e.GetCH0, e.GetCH1, e.GetCH2, e.GetCH3 };
            draw_soundmap(power);
        }

        public void draw_soundmap(Int16[] power, int n = 1000)
        {
            //double[,] data = Calculations.getDouble(Calculations.calculate_map(power, n));
            //var heatMapSeries = new HeatMapSeries
            //{
            //    X0 = 0,
            //    X1 = n - 1,
            //    Y0 = 0,
            //    Y1 = n - 1,
            //    Interpolate = false,
            //    RenderMethod = HeatMapRenderMethod.Bitmap,
            //    Data = data
            //};
            //var model = new PlotModel { Title = "Sound Map" };
            //model.Axes.Add(new LinearColorAxis
            //{
            //    Palette = OxyPalettes.Rainbow(100)
            //});
            //model.Series.Add(heatMapSeries);
            //plot1.Model = model;

            Int16[,] data = Calculations.calculate_map(power, n);
            byte r, g, b;
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    b = (byte)(255 - data[i, j]);
                    g = (byte)(4 *  data[i, j] );
                    r = (byte)(8 *  data[i, j] );
                    bitmap.SetPixel(j, i, Color.FromArgb(r, g, b));
                }
            }
            pictureBox1.Image = bitmap;
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Int16[] datach0 = datah.read_ch0();
            Int16[] datach1 = datah.read_ch1();
            Int16[] datach2 = datah.read_ch2();
            Int16[] datach3 = datah.read_ch3();
            if (datach0!=null && datach1 != null && datach2 != null && datach3 != null)
            {
                // calculate fft
                //double[] fft1 = Calculations.calculate_fft(datach0);
                //double[] fft2 = Calculations.calculate_fft(datach1);
                //double[] fft3 = Calculations.calculate_fft(datach2);
                //double[] fft4 = Calculations.calculate_fft(datach3);

                //Int16[] power = Calculations.calculate_peak_power(fft1, fft2, fft3, fft4);
                //Int16[] power = { (Int16)datach0.Average(x => x), (Int16)datach0.Average(x => x), (Int16)datach0.Average(x => x), (Int16)datach0.Average(x => x) };

                Int16[] power = { datach0[0], datach1[0], datach2[0], datach3[0] };
                status_update("power: " + power[0] + " , " + power[1] + " , " + power[2] + " , " + power[3]);
                draw_soundmap(power);
            }
        }
    }
}
