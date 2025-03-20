using System.Net.Http;
using System.Text;
using CheckersBot.logic;
using Newtonsoft.Json;

namespace CheckersBot.utils;

public delegate void OnMessageReceived(string message);
public class ApiCalls
{
    private static readonly string ApiKey = Environment.GetEnvironmentVariable("GPT_SECRET_KEY")!;
    private static readonly string ApiUrl = Environment.GetEnvironmentVariable("API_URl")!;
    public static event OnMessageReceived OnMessageReceived = null!;
    public static async Task CallChatGptForHelp(Board board)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "You are a bot for checkers." },
                new
                {
                    role = "user", content = "can you please recommend move for"+board.ColorToMove+" with board\n"+
                                             board + "Where --- is empty square, MWh - white man piece," +
                                             " KWh - white king piece," +
                                             "MBl - black man piece KBl - black king piece",
                }
            }
        };

        string jsonRequest = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(ApiUrl, content);
        string responseString = await response.Content.ReadAsStringAsync();
        OnMessageReceived(responseString);
    }
}