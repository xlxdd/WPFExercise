using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Text;
using System.Windows;

namespace com;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string sendData;
    [ObservableProperty]
    private string receiveData;
    //日志
    private StringBuilder _logBuilder = new StringBuilder();
    public string Log
    {
        get { return _logBuilder.ToString(); }
    }
    private void AppendLog(string message)
    {
        _logBuilder.AppendLine(message);
        OnPropertyChanged(nameof(Log));
    }
    [ObservableProperty]
    private ObservableCollection<string> portName;
    [ObservableProperty]
    private string selectedPort;

    [ObservableProperty]
    private List<int> baudRate;
    [ObservableProperty]
    private int selectedBaudRate;

    [ObservableProperty]
    private List<int> dataBits;
    [ObservableProperty]
    private int selectedDataBits;

    [ObservableProperty]
    private List<string> stopBits;
    [ObservableProperty]
    private string selectedStopBits;
    [ObservableProperty]

    private List<string> parity;
    [ObservableProperty]
    private string selectedParity;

    private SerialPort serialPort;
    public MainViewModel()
    {
        ExcuteCommand = new RelayCommand<string>(Excute);
        PortName = new();
        BaudRate = new List<int>() { 2400, 4800, 9600, 19200, 38400, 57600, 115200 };
        DataBits = new List<int>() { 5, 7, 8 };
        StopBits = new List<string>() { "None", "One", "Two", "OnePointFive" };
        Parity = new List<string>() { "None", "Odd", "Even", "Mark", "Space" };
        var serialPort = new SerialPort();
    }

    private void Excute(string option)
    {
        switch (option)
        {
            case "check":
                Check();
                break;
            case "open":
                Open();
                break;
            case "close":
                Close();
                break;
            case "send":
                Send();
                break;
            case "clearSend":
                SendData = "";
                break;
            case "clearRec":
                ReceiveData = "";
                break;
        }
    }

    public RelayCommand<string> ExcuteCommand { get; set; }
    private void Check()
    {
        AppendLog(DateTime.Now + "串口检测开始");
        PortName.Clear();
        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
            PortName.Add(port);
        AppendLog(DateTime.Now + "串口检测完成");
    }
    private void Open()
    {
        if (string.IsNullOrWhiteSpace(SelectedPort))
        {
            MessageBox.Show("未选择串口", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        try
        {
            serialPort = new SerialPort();
            serialPort.PortName = SelectedPort;//连接端口
            serialPort.BaudRate = SelectedBaudRate;//设置波特率
            serialPort.DataBits = SelectedDataBits;//设置数据位
            serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), SelectedStopBits);//设置停止位
            serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), SelectedParity);//设置校验位
            serialPort.Open();
            if (serialPort.IsOpen)
                AppendLog(DateTime.Now + $"串口{SelectedPort}打开成功");
            //开始监听
            serialPort.DataReceived += SerialPort_DataReceived;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            int count = serialPort.ReadByte();
            Byte[] buffer = new Byte[count];
            serialPort.Read(buffer, 0, count);

            ReceiveData = ReceiveData + DateTime.Now + "\t" + Encoding.UTF8.GetString(buffer) + "\r\n";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void Close()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
            AppendLog(DateTime.Now + $"串口{SelectedPort}关闭成功");
        }
        else
            MessageBox.Show("未选择串口", "error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    private void Send()
    {
        if (null == serialPort)
        {
            MessageBox.Show("未选择串口", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (serialPort.IsOpen)
        {
            Task.Run(() =>
            {
                try
                {
                    //设置超时时间
                    serialPort.WriteTimeout = 5000;
                    serialPort.WriteLine(DateTime.Now + "\t" + SendData + "\r");
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        AppendLog(DateTime.Now + $"{SelectedPort} 发送--->>> {SendData}");
                    });
                }
                catch
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        AppendLog(DateTime.Now + $"{SelectedPort} 发送超时");
                    });
                }
            });
        }
    }
}
