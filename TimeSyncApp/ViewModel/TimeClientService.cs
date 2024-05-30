using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;


namespace TimeSyncApp.ViewModel
{
    public class TimeClientService
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Milliseconds;
        }
        public async Task<bool> SynchronizeTimeAsync(string serverIp, int port)
        {
            DateTime? serverTimeUtc = null;
            using (var udpClient = new UdpClient())
            {
                var data = new byte[1024];
                await udpClient.SendAsync(data, data.Length, serverIp, port);
                var receivedResults = await udpClient.ReceiveAsync();
                var timeData = receivedResults.Buffer;
                var serverTimeTicks = BitConverter.ToInt64(timeData, 0);
                serverTimeUtc = DateTime.FromBinary(serverTimeTicks).ToUniversalTime();
            }

            if (serverTimeUtc.HasValue)
            {

                var systime = new SYSTEMTIME
                {
                    Year = (ushort)serverTimeUtc.Value.Year,
                    Month = (ushort)serverTimeUtc.Value.Month,
                    Day = (ushort)serverTimeUtc.Value.Day,
                    Hour = (ushort)serverTimeUtc.Value.Hour,
                    Minute = (ushort)serverTimeUtc.Value.Minute,
                    Second = (ushort)serverTimeUtc.Value.Second,
                    Milliseconds = (ushort)serverTimeUtc.Value.Millisecond
                };

                return SetSystemTime(ref systime);
            }
            else
            {
                return false;
            }
        }
    }
}
