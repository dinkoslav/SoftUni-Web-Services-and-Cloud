namespace Battleships.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CommandProcessor
    {
        private const string RegisterNewUserEndpoint = "http://localhost:62859/api/Account/Register";
        private const string LoginUserEndpoint = "http://localhost:62859/Token";
        private const string JoinGameEndpoint = "http://localhost:62859/api/games/join";
        private const string PlayTurnEndpoint = "http://localhost:62859/api/games/play";
        private const string CreateGameEndpoint = "http://localhost:62859/api/games/create";

        private string token = "";
        private string gameId = "";

        public CommandProcessor()
        {
            
        }

        public bool ProcessCommand(string commandLine)
        {
            string[] commandParameters = commandLine.Split(' ');
            string command = commandParameters[0];
            switch (command)
            {
                case "$register":
                    RegisterUserAsync(commandParameters[1], commandParameters[2], commandParameters[3]);
                    return true;
                case "$login":
                    LoginUserAsync(commandParameters[1], commandParameters[2]);
                    return true;
                case "$create-game":
                    CreateGameAsync();
                    return true;
                case "$join-game":
                    JoinGameAsync(commandParameters[0]);
                    return true;
                case "$play":
                    PlayAsync(commandParameters[1], commandParameters[2], commandParameters[3]);
                    return true;
                case "$exit":
                    return false;
                default:
                    Console.Write("Invalid command, please enter again :");
                    return true;
            }
        }

        public async void RegisterUserAsync(string email, string password, string confirmPassword)
        {
            var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("Email", email),
                new KeyValuePair<string, string>("Password", password),
                new KeyValuePair<string, string>("ConfirmPassword", confirmPassword)
                });

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(RegisterNewUserEndpoint, content);
                Console.WriteLine(response.IsSuccessStatusCode ? "User registered." : "Error registering. Try again.");
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async void LoginUserAsync(string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Username", username), 
                    new KeyValuePair<string, string>("Password", password), 
                    new KeyValuePair<string, string>("grant_type", "password"), 
                });
                var response = await httpClient.PostAsync(LoginUserEndpoint, content);

                string result;
                if (!response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    token = (await response.Content.ReadAsAsync<LoginDTO>()).Access_Token;
                }
            }
        }

        public async void CreateGameAsync()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var response = await httpClient.PostAsync(CreateGameEndpoint, null);

                gameId = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("You just created a game !!! lol nice ;)");
                }
            }
        }

        public async void JoinGameAsync(string gameId)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("GameId", gameId)
                });
                var response = await httpClient.PostAsync(JoinGameEndpoint, content);

                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("You are in !!! ... Smash ogre ...");
                }
            }
        }

        public async void PlayAsync(string gameId, string posX, string posY)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("GameId", gameId),
                    new KeyValuePair<string, string>("PositionX", posX),
                    new KeyValuePair<string, string>("PositionY", posY)
                });

                var response = await httpClient.PostAsync(PlayTurnEndpoint, content);

                string result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Something is wrong ... try again on different position ;)");
                }

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("You moved ... YOU MOVED!!!! ");
                }
            }
        }
    }

    public class LoginDTO
    {
        public string Access_Token { get; set; }
    }
}
