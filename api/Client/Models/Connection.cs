using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Models.Account;
using Client.Models.Courses;
using Client.ViewModels;
using Newtonsoft.Json;
using static Client.ViewModels.MainWindowViewModel;

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

        private string ParseErrorResponse(string jsonResponse)
        {
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ApiErrorResponse>(jsonResponse);

                var errorMessage = new StringBuilder();
                if (errorResponse.Errors != null)
                {
                    foreach (var error in errorResponse.Errors)
                    {
                        errorMessage.AppendLine($"{string.Join(", ", error.Value)}");
                    }
                }
                return errorMessage.Length > 0 ? errorMessage.ToString() : errorResponse.Title;
            }
            catch
            {
                return jsonResponse;
            }
        }

        public async Task<string> LogInUser(LogInDTO loginEnter)
        {
            JsonContent loginEnterSerialize = JsonContent.Create(loginEnter);

            HttpResponseMessage response = await Client.PostAsync("Account/Login", loginEnterSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MainWindowViewModel.Instance.ErrorMessage("Ошибка регистрации!", response.Content.ToString());
            }

            return responseBody;
        }
        public async Task<string> GetAllUsers(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await Client.GetAsync("User/GetAllUsers");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MainWindowViewModel.Instance.ErrorMessage("Не удалось получить пользователей!", response.Content.ToString());
            }

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

        public async Task<string> GetCourses()
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",MainWindowViewModel.Instance.CurrentUser.Token);
            HttpResponseMessage response = await Client.GetAsync("Course/GetAuthorsForCourses");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MainWindowViewModel.Instance.ErrorMessage("Не удалось получить курсы!", response.Content.ToString());
            }

            return responseBody;
        }

        public async Task<string> AddCourse(AddCourseDTO newCourse)
        {
            Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.Instance.CurrentUser.Token);
            JsonContent newCourseSerialize = JsonContent.Create(newCourse);
            HttpResponseMessage response = await Client.PostAsync("Course/AddCourse", newCourseSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                MainWindowViewModel.Instance.ErrorMessage("Ошибка добавления курса!", ParseErrorResponse(responseBody));
            }


            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateCourse(UpdateCourseDTO newCourse)
        {
            JsonContent newCourseSerialize = JsonContent.Create(newCourse);
            HttpResponseMessage response = await Client.PutAsync("Course/UpdateCourse", newCourseSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MainWindowViewModel.Instance.ErrorMessage("Ошибка обновления курса!", ParseErrorResponse(responseBody));
            }

            return responseBody;
        }

        public async Task<string> GetBlock(ShortCourseDTO courseInfo)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.Instance.CurrentUser.Token);
            HttpResponseMessage response = await Client.GetAsync($"Block/GetBlockOfCourse?courseId={courseInfo.CourseId}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MainWindowViewModel.Instance.ErrorMessage("Ошибка получения блока!", response.Content.ToString());
            }

            return responseBody;
        }
    }
}
