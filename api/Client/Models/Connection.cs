using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Client.Models.Account;
using Client.Models.Courses;
using Client.Models.Users;
using Client.ViewModels;
using Newtonsoft.Json.Linq;

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

        public async Task<string> GetAllUsers(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await Client.GetAsync("User/GetAllUsers");
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<string> GetAllCourses(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.Instance.CurrentUser.Token);
            HttpResponseMessage response = await Client.GetAsync("Course/GetAllCourses");

            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<string> GetAuthors()
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",MainWindowViewModel.Instance.CurrentUser.Token);
            HttpResponseMessage response = await Client.GetAsync("Course/GetAuthorsForCourses");
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<string> AddCourse(AddCourseDTO newCourse)
        {
            Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.Instance.CurrentUser.Token);
            JsonContent newCourseSerialize = JsonContent.Create(newCourse);
            HttpResponseMessage responce = await Client.PostAsync("Course/AddCourse", newCourseSerialize);
            return await responce.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateCourse(UpdateCourseDTO newCourse)
        {
            Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.Instance.CurrentUser.Token);
            JsonContent newCourseSerialize = JsonContent.Create(newCourse);
            HttpResponseMessage responce = await Client.PutAsync("Course/UpdateCourse", newCourseSerialize);
            return await responce.Content.ReadAsStringAsync();
        }
    }
}
