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
using UnityEngine;

namespace Assets.Scripts.Communication.Udp
{
    public class UdpWobbleboardService : WobbleboardFetcher
    {
        // Private fields
        private UdpClient _udpClient;



        // Initialization
        private void Awake()
        {
            _udpClient = new UdpClient(8000);                
        }



        // Core
        protected override async UniTask<T> Get<T>(T responseSchema = default)
        {
            var data = await _udpClient.ReceiveAsync();
            Debug.Log("Dati");
            var json = Encoding.UTF8.GetString(data.Buffer);
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected override void OnDestroy()
        {            
            CloseUdp();
            base.OnDestroy();
        }

        private void OnDisable()
        {
            _udpClient.Close();            
            _udpClient.Dispose();
        }



        // Utils
        private void CloseUdp()
        {
            if (_udpClient == null)
                return;

            _udpClient.Close();
            _udpClient.Dispose();
        }
    }
}
