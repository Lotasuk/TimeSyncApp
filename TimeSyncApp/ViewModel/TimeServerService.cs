using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;


namespace TimeSyncApp.ViewModel
{
    public class TimeServerService
    {
        public async Task StartServerAsync()
        {
            using (var udpClient = new UdpClient(32122))
            {
                int selectedPort = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;
                while (true)
                {
                    var receivedResults = await udpClient.ReceiveAsync();
                    var clientEndPoint = receivedResults.RemoteEndPoint;
                    var currentTime = DateTime.Now.ToBinary();
                    var data = BitConverter.GetBytes(currentTime);
                    await udpClient.SendAsync(data, data.Length, clientEndPoint);
                }              
            }          
        }
    }
}
