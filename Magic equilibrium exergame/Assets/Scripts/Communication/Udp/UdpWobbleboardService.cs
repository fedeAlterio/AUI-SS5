using Assets.Scripts.Abstractions;
using Assets.Scripts.Communication.Abstractions;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Communication.Udp
{
    public class UdpWobbleboardService : WobbleboardFetcher
    {
        // Private fields
        private static readonly UdpClient _udpClient = new UdpClient(8000);



        // Core
        protected override async UniTask<T> Get<T>(T responseSchema = default)
        {
            var data = await _udpClient.ReceiveAsync();
            var json = Encoding.UTF8.GetString(data.Buffer);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
