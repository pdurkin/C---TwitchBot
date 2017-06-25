using System;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;
using TwitchLib.Models.API.v5.Users;

//Basic implementation of a Twitch Chat Bot, as originally demonstrated by Hardly Difficult in his youtube video "Twitch Chat bot using C# and TwitchLib" with custom features added by myself

namespace TwitchBot
{
    class Program
    {
        static void Main(string[] args)
        {
            TwitchChatBot bot = new TwitchChatBot();
            bot.Connect();

            Console.ReadLine();

            bot.Disconnect();
            
        }
    }
}
