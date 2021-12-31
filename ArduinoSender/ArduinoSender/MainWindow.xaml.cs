using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace ArduinoSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SerialPort CurrentSerialPort { get; set; }
        public string SerialPortName { get; set; }
        public string SerialDataRecieved { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //Subscribe to Loaded event
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                ComboBoxPort.Items.Add(port);
            }
            ComboBoxBaudRate.Items.Add("9600");
            ComboBoxBaudRate.SelectedValue = "9600";


            ButtonConnectUsb.IsEnabled = true;
            ButtonDisconnectUsb.IsEnabled = false;
            ButtonSendData.IsEnabled = false;
        }
        void OnClickConnectUsbPort(object sender, RoutedEventArgs e)
        {
            if (CurrentSerialPort != null)
            {
                if (CurrentSerialPort.IsOpen)
                {
                    CurrentSerialPort.Close();
                }
            }

            if (string.IsNullOrEmpty(ComboBoxPort.SelectedItem.ToString()))
            {
                MessageBox.Show("You have to select a Port!");
            }

            ButtonDisconnectUsb.IsEnabled = true;
            ButtonSendData.IsEnabled = true;
            SerialPortName = ComboBoxPort.SelectedItem.ToString();
            CurrentSerialPort = new SerialPort(SerialPortName);
            CurrentSerialPort.BaudRate = Convert.ToInt32(ComboBoxBaudRate.SelectedItem);
            try
            {
                CurrentSerialPort.DataReceived += new SerialDataReceivedEventHandler(DataRecievedHandler);
                CurrentSerialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            for (var value = 0; value<=100; value++)
            {
                ProgressBarConnecting.Value = value;
            }
        }

        void OnClickDisconnectUsbPort(object sender, RoutedEventArgs e)
        {
            if (CurrentSerialPort == null)
            {
                return;
            }
            if (!CurrentSerialPort.IsOpen)
            {
                return;
            }
            ButtonDisconnectUsb.IsEnabled = false;
            ButtonSendData.IsEnabled = false;
            ProgressBarConnecting.Value = 0;

            CurrentSerialPort.Close();
            SerialPortName = "";
        }
        void DataRecievedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialDataRecieved = CurrentSerialPort.ReadExisting();
            this.Dispatcher.Invoke(() => { 
                TextBoxRecievedData.Text += SerialDataRecieved; 
            });
        }

        void OnClickSendData(object sender, RoutedEventArgs e)
        {
            if (CurrentSerialPort == null)
            {
                return;
            }
            try
            {
                CurrentSerialPort.Write(TextBoxDataSend.Text + "#");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
