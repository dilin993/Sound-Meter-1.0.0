using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace Sound_Meter_1._0._0
{
    public delegate void BlockReceivedEventHandler(object sender, CommunicationHandlerEventArgs e);

    public class CommunicationHandlerEventArgs : EventArgs
    {
        private int[] datach = { 0, 0, 0, 0 };

        public CommunicationHandlerEventArgs(int[] datach)
        {
            for (int i = 0; i < 4; i++)
            {
                this.datach[i] = datach[i];
            }
        }

        public int GetCH0
        {
            get { return datach[0]; }
        }

        public int GetCH1
        {
            get { return datach[1]; }
        }

        public int GetCH2
        {
            get { return datach[2]; }
        }

        public int GetCH3
        {
            get { return datach[3]; }
        }

        public int[] GetChannels
        {
            get { return datach; }
        }
    }

    class CommunicationHandler
    {
        public event BlockReceivedEventHandler received;
        static SerialPort serialPort;
        static PacketTypes nextPacket = PacketTypes.CH0L;
        static int[] datach = { 0, 0, 0, 0 };
        private string port;
        private int baudrate;

        enum PacketTypes
        {
            CH0L = 0,
            CH1L = 1,
            CH2L = 2,
            CH3L = 3,
            CH0H = 4,
            CH1H = 5,
            CH2H = 6,
            CH3H = 7,
        };

        public void start()
        {
            var portExists = SerialPort.GetPortNames().Any(x => x == port);
            if (portExists)
            {
                try
                {
                    serialPort = new SerialPort();
                    serialPort.PortName = port;
                    serialPort.BaudRate = baudrate;
                    serialPort.Parity = Parity.None;
                    serialPort.DataBits = 8;
                    serialPort.StopBits = StopBits.One;
                    serialPort.Handshake = Handshake.None;

                    serialPort.ReadTimeout = 500;
                    serialPort.WriteTimeout = 500;

                    serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    serialPort.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
                throw new Exception("Port '" + port + "' does not exist.");
           
        }

        public void stop()
        { 
            serialPort.Close();
        }

        protected virtual void OnReceived(CommunicationHandlerEventArgs e)
        {
            received?.Invoke(this, e);
        }

        private void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
        {
            int raw, data;
            PacketTypes code;
            SerialPort sp = (SerialPort)sender;
            while (sp.BytesToRead > 0)
            {
                raw = sp.ReadByte();
                data = raw & 31;
                code = (PacketTypes)((raw & 224) >> 5);

                switch (nextPacket)
                {
                    case PacketTypes.CH0L:
                        if (code == PacketTypes.CH0L)
                        {
                            datach[0] = data;
                            nextPacket = PacketTypes.CH0H;
                        }
                        break;

                    case PacketTypes.CH0H:
                        if (code == PacketTypes.CH0H)
                        {
                            datach[0] += (data << 5);
                            nextPacket = PacketTypes.CH1L;
                        }
                        else
                            nextPacket = PacketTypes.CH0L;
                        break;

                    case PacketTypes.CH1L:
                        if (code == PacketTypes.CH1L)
                        {
                            datach[1] = data;
                            nextPacket = PacketTypes.CH1H;
                        }
                        else
                            nextPacket = PacketTypes.CH0L;
                        break;

                    case PacketTypes.CH1H:
                        if (code == PacketTypes.CH1H)
                        {
                            datach[1] += (data << 5);
                            nextPacket = PacketTypes.CH2L;
                        }
                        else
                            nextPacket = PacketTypes.CH0L;
                        break;

                    case PacketTypes.CH2L:
                        if (code == PacketTypes.CH2L)
                        {
                            datach[2] = data;
                            nextPacket = PacketTypes.CH2H;
                        }
                        else
                            nextPacket = PacketTypes.CH0L;
                        break;

                    case PacketTypes.CH2H:
                        if (code == PacketTypes.CH2H)
                        {
                            datach[2] += (data << 5);
                            nextPacket = PacketTypes.CH3L;
                        }
                        else
                            nextPacket = PacketTypes.CH0L;
                        break;

                    case PacketTypes.CH3L:
                        if (code == PacketTypes.CH3L)
                        {
                            datach[3] = data;
                            nextPacket = PacketTypes.CH3H;
                        }
                        else
                            nextPacket = PacketTypes.CH0L;
                        break;

                    case PacketTypes.CH3H:
                        if (code == PacketTypes.CH3H)
                        {
                            datach[3] += (data << 5);
                            OnReceived(new CommunicationHandlerEventArgs(datach));
                        }
                        nextPacket = PacketTypes.CH0L;
                        break;
                }
            }
        }


        public CommunicationHandler(string port, int baudrate)
        {
            this.port = port;
            this.baudrate = baudrate;
        }

        ~CommunicationHandler()
        {
            serialPort.Close();
        }
    }
}
