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
        private const int WINDOW_SIZE = 10;
        OxyPlot.WindowsForms.PlotView plot1;

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

            // Add oxyplot components
            plot1 = new OxyPlot.WindowsForms.PlotView();
            this.SuspendLayout();
            // 
            // plot1
            // 
            plot1.Dock = System.Windows.Forms.DockStyle.Fill;
            plot1.Name = "plot1";
            plot1.PanCursor = System.Windows.Forms.Cursors.Hand;
            plot1.Text = "plot1";
            plot1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            plot1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            plot1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            pnlMap.Controls.Add(plot1);

            int[] power = { 20,20,15,15};
            draw_soundmap(power);
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
                this.txtStatus.Text += DateTime.Now.ToString("hh:mm:ss tt") + ">>> " + text +"\r\n";
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
                    timer1.Enabled = true;
                    timer1.Start();
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
                timer1.Stop();
                timer1.Enabled = false;
            }
            
        }

        private void OnDataCollected(object sender, CommunicationHandlerEventArgs e)
        {
            datah.enqueue(e.GetChannels);
            string status = "CH0: " + e.GetCH0 + ", " +
                              "CH1: " + e.GetCH1 + ", " +
                              "CH2: " + e.GetCH2 + ", " +
                              "CH3: " + e.GetCH3;
            status_update(status);
        }

        public void draw_soundmap(int[] power, int n=1000)
        {
            double[,] data = Calculations.calculate_map(power, n);
            var heatMapSeries = new HeatMapSeries
            {
                X0 = 0,
                X1 = n - 1,
                Y0 = 0,
                Y1 = n - 1,
                Interpolate = true,
                RenderMethod = HeatMapRenderMethod.Bitmap,
                Data = data
            };
            var model = new PlotModel { Title = "Sound Map" };
            model.Axes.Add(new LinearColorAxis
            {
                Palette = OxyPalettes.Rainbow(100)
            });
            model.Series.Add(heatMapSeries);
            plot1.Model = model;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int[] datach0 = datah.read_ch0();
            if(datach0!=null)
            {
                string tmp = "";
                for (int i = 0; i < WINDOW_SIZE; i++)
                {
                    tmp += datach0[i].ToString() + " ";
                }
                status_update("Block: " + tmp);
            }
        }
    }
}
