using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Frontend;
using Backend.Entities;

namespace Test
{
	class Program
	{
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
            RunAsync().GetAwaiter().GetResult();
        }

        static void ShowUser(Data user)
		{
            if (user != null)
                Console.WriteLine($"{user.Username} {user.Name} {user.Surname}");
            else
                Console.WriteLine("No user");
		}

        static void ShowUser(User user)
        {
            if (user != null)
                Console.WriteLine($"{user.Username} {user.Name} {user.Surname}");
            else
                Console.WriteLine("No user");
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            //client.BaseAddress = new Uri("https://localhost:5001/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                //Get all users
                List<User> users = await Client.GetAllUsersAsync();

				foreach (User u in users)
					ShowUser(u);

				Console.WriteLine();

                //Get existing user
                Data user = await Client.GetExistingUserAsync(new SignIn {Username = "george3", Password = "123" });
                ShowUser(user);

                //check potential user
                //bool value = await Client.CheckPotentialUserAsync(new SignUp { Username = "emma"});
                bool value = await Client.CheckPotentialUserAsync(new SignUp { Username = "rose"});
                Console.WriteLine(value);

                user = null;
                if(value)
				{
                    user = await Client.AddUserAsync(new User {Username = "rose", Name = "Emily", Surname = "Black", Password = "123"});
				}
                ShowUser(user);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

    }
}
