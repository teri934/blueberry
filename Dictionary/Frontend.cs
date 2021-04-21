using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DTO;

namespace Dictionary
{
	class Frontend
	{
		const string GetAll = "users/getAll";
		const string GetExisting = "users/getExisting";
		const string CheckPotential = "users/checkPotential";
		const string AddUser = "users/add";

		static HttpClient client = new HttpClient();

		public static async Task<List<DTOUser>> GetAllUsersAsync()
		{
			List<DTOUser> users = null;
			HttpResponseMessage response = await client.GetAsync(GetUri(GetAll));
			if (response.IsSuccessStatusCode)
			{
				users = await response.Content.ReadAsAsync<List<DTOUser>>();
			}
			return users;
		}

		public static async Task<DTORelevantData> GetExistingUserAsync(DTOSignIn signin)
		{
			DTORelevantData data = null;
			HttpResponseMessage response = await client.PostAsJsonAsync(GetUri(GetExisting), signin);
			response.EnsureSuccessStatusCode();

			data = await response.Content.ReadAsAsync<DTORelevantData>();
			return data;
		}

		public static async Task<bool> CheckPotentialUserAsync(DTOSignUp signup)
		{
			HttpResponseMessage response = await client.PostAsJsonAsync(GetUri(CheckPotential), signup);
			response.EnsureSuccessStatusCode();

			bool value = await response.Content.ReadAsAsync<bool>();
			return value;
		}

		public static async Task<DTORelevantData> AddUserAsync(DTOUser user)
		{
			HttpResponseMessage response = await client.PostAsJsonAsync(GetUri(AddUser), user);
			response.EnsureSuccessStatusCode();

			DTORelevantData data = await response.Content.ReadAsAsync<DTORelevantData>();
			return data;
		}

		static Uri GetUri(string path) => new Uri("https://localhost:5001/" + path);
	}
}