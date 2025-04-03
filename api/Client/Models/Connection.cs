using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class Connection
    {

        private HttpClient _client;

        public Connection(string baseUri)
        {
            Client = new HttpClient();
            Client.BaseAddress = new System.Uri(baseUri);
        }

        public HttpClient Client
        {
            get => _client;
            set => _client = value;
        }

        public async Task<string> Ge
    }
}
