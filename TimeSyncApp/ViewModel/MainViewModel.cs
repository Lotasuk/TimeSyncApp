using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System;


namespace TimeSyncApp.ViewModel
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private readonly TimeServerService _timeServerService = new TimeServerService();
        private readonly TimeClientService _timeClientService = new TimeClientService();

        string hostName = Dns.GetHostName();
        

        private string _serverIpAddress;
        public string ServerIpAddress
        {
            get => _serverIpAddress;
            set
            {
                _serverIpAddress = value;
                OnPropertyChanged(nameof(ServerIpAddress));
            }
        }
        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }
        private string _serverPort;
        public string ServerPort
        {
            get => _serverPort;
            set
            {
                _serverPort = value;
                OnPropertyChanged(nameof(ServerPort));
            }
        }
        private bool _isServerRunning;
        public bool IsServerRunning
        {
            get => _isServerRunning;
            set
            {
                _isServerRunning = value;
                OnPropertyChanged(nameof(IsServerRunning));
            }
        }

        public ICommand StartServerCommand { get; }
        public ICommand SynchronizeTimeCommand { get; }
        public ICommand ClientEnterCommand { get; }


        public MainViewModel()
        {
            StartServerCommand = new RelayCommand(StartServer);
            SynchronizeTimeCommand = new RelayCommand(SynchronizeTime);
            ClientEnterCommand = new RelayCommand(ClientEnterAction);
           

        }

        private void ClientEnterAction()
        {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            
            var client = new Client();
            client.Show();

            
            if (mainWindow != null)
            {
                mainWindow.Close();
            }
        }

        private async void StartServer()
        {
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);
            IPAddress ipv4Address = addresses.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            StatusMessage = $"Сервер запущен {Environment.NewLine}IP: {ipv4Address}{Environment.NewLine}Порт: 32122";
            IsServerRunning = true;

            await _timeServerService.StartServerAsync();          
        }

        private async void SynchronizeTime()
        {
            if (!string.IsNullOrEmpty(ServerIpAddress) && !string.IsNullOrEmpty(ServerPort))
            {
                int port;
                if (int.TryParse(ServerPort, out port))
                {
                    var serverTime = await _timeClientService.SynchronizeTimeAsync(ServerIpAddress, port);
                    if (serverTime)
                    {
                        MessageBox.Show("Время синхронизировано.");

                    }
                    else
                    {
                        MessageBox.Show("Не удалось синхронизировать время.");
                    }
                }
                else
                {
                    MessageBox.Show("Порт введен неверно.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста введите IP адрес и порт.");
            }
        }


    }
}