using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ConnectionApi
    {

        private HttpClient _client;

        public ConnectionApi(string baseUri)
        {
            Client = new HttpClient();
            Client.BaseAddress = new System.Uri(baseUri);
        }

        public HttpClient Client
        {
            get => _client;
            set => _client = value;
        }
        public async Task<HttpStatusCode> CheckAvailability()
        {
            HttpResponseMessage response = await Client.GetAsync("Connection/CheckConnection");
            return response.StatusCode;
        }
    }
}
