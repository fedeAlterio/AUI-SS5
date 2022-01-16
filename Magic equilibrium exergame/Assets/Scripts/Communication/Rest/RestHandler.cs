using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine.Networking;

namespace Assets.Scripts.Communication
{
    /// <summary>
    /// This class exposes generic method to Post and Get to a Rest Service.
    /// </summary>
    public class RestHandler
    {
        protected static Uri _serverUri;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();



        // Initialization
        public RestHandler(Uri uri, string baseUrl)
        {
            _serverUri = uri;
            BaseUrl = baseUrl;
        }



        // Properties        
        protected string BaseUrl { get; set; }
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();



        // Public
        public void Abort()
        {
            var tcs = _cancellationTokenSource;
            _cancellationTokenSource = new CancellationTokenSource();
            tcs.Cancel();
            tcs.Dispose();
        }


        // Post
        /// <summary>
        /// Does a Post request, parses the response and returns an object.
        /// </summary>
        /// <typeparam name="T">Type of the object the method converts the response into</typeparam>
        /// <param name="body">Body of the Post request. This is sent as json object in the HTTP Body</param>
        /// <param name="responseSchema">Dummy parameter used to allow anonymous classes to be used as return types</param>
        /// <param name="remoteActionName">Remote action name of the Rest API</param>
        /// <returns>The parsed object</returns>
        public async UniTask<T> PostAndGet<T>(object body, T responseSchema = default, [CallerMemberName] string remoteActionName = "", CancellationToken cancellationToken = default)
        {
            var uri = ActionNameToUri(remoteActionName);
            var response = await PostWithUri(uri, body, cancellationToken);

            var ret = ParseResponse<T>(response);
            return ret;
        }


        public async UniTask Post(object body, [CallerMemberName] string remoteActionName = "", CancellationToken cancellationToken = default)
        {
            var uri = ActionNameToUri(remoteActionName);
            var response = await PostWithUri(uri, body, cancellationToken);
        }


        public async UniTask<string> PostWithUri(Uri relativeUri, object toSend, CancellationToken cancellationToken)
        {
            try
            {
                var uri = new Uri(_serverUri, relativeUri);
                var jsonString = JsonConvert.SerializeObject(toSend);

                var request = new UnityWebRequest(uri.AbsoluteUri, "POST");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                // Set the headers
                request.SetRequestHeader("Content-Type", "application/json");
                foreach (var x in Headers)
                    request.SetRequestHeader(x.Key, x.Value);

                // Sending the request and wait for the response
                var response = await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

                // return the raw string response body
                return response.downloadHandler.text;
            }
            catch (UnityWebRequestException e)
            {
                throw GameException(e);
            }
        }




        // Parse Response
        private static T ParseResponse<T>(string jsonResponse)
        {
            // Read response as a string
            //var responseJson = await respone.Content.ReadAsStringAsync();

            // Transform the string into a class throgh Json Converter
            var ret = JsonConvert.DeserializeObject<T>(jsonResponse);
            return ret;
        }



        // Helpers
        protected GameException GameException(UnityWebRequestException unityException)
        {
            var startIndex = unityException.Message.IndexOf("error", 0, 3);
            var quote = '"';
            var regex = new Regex(@$"error{quote}\:{quote}([^{quote}]*){quote}");
            var match = regex.Match(unityException.Message);
            var message = match.Groups.Count >= 2 ? match.Groups[1].Value : unityException.Message;

            return new GameException(message);
        }

        /// <summary>
        /// Converts A relative uri into a full uri
        /// </summary>
        /// <returns>Full uri</Rreturns>
        protected Uri ActionNameToUri(string name) => ToRelativeUri(BaseUrl + name);
        protected static Uri ToRelativeUri(string uri) => new Uri(uri, UriKind.Relative);
    }
}
