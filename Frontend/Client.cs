using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Backend.Entities;

namespace Frontend
{
	public class Client
	{
		static HttpClient client = new HttpClient();

        public static async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = null;
            HttpResponseMessage response = await client.GetAsync(GetUri("users/getAll"));
            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<User>>();
            }
            return users;
        }

        public static async Task<Data> GetExistingUserAsync(SignIn signin)
        {
            Data data = null;
            HttpResponseMessage response = await client.PostAsJsonAsync(GetUri("users/getExisting"), signin);
            response.EnsureSuccessStatusCode();

            data = await response.Content.ReadAsAsync<Data>();
            return data;
        }

        public static async Task<bool> CheckPotentialUserAsync(SignUp signup)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(GetUri("users/checkPotential"), signup);
            response.EnsureSuccessStatusCode();

            bool value = await response.Content.ReadAsAsync<bool>();
            return value;
        }

        public static async Task<Data> AddUserAsync(User user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(GetUri("users/add"), user);
            response.EnsureSuccessStatusCode();

            Data data = await response.Content.ReadAsAsync<Data>();
            return data;
        }

        static Uri GetUri(string path) => new Uri("https://localhost:5001/" + path);
    }
}
