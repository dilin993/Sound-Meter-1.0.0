using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Sound_Meter_1._0._0
{
    public partial class frmSettings : Form
    {
        String[] baudrates = { "9600", "19200", "38400", "57600", "14400",
                            "28800", "56000", "115200" };
        private int baudrate;
        private string port;

        public int Baudrate
        {
            get { return baudrate; }
        }

        public string PortName
        {
            get { return port; }
        }

        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            cbPort.Items.AddRange(SerialPort.GetPortNames());
            cbBaudrate.Items.AddRange(baudrates);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool portExists = SerialPort.GetPortNames().Any(x => x == cbPort.Text);
            int n;
            bool isNumeric = int.TryParse(cbBaudrate.Text, out n);
            if (portExists && isNumeric)
            {
                port = cbPort.Text;
                baudrate = n;
                this.Close();
            }
            else
            {
                MessageBox.Show("Entered settings are invalid.", "Invalid Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
