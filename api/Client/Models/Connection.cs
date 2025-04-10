using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Client.Models.Account;

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

        public async Task<string> LogInUser(LogInDTO loginEnter)
        {

            JsonContent loginEnterSerialize = JsonContent.Create(loginEnter);

            HttpResponseMessage response = await Client.PostAsync("Account/Login", loginEnterSerialize);

            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}
