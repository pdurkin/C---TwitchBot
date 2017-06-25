using System;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;
using TwitchLib.Models.API.v5.Users;
using System.Collections.Generic;
using System.Timers;

namespace TwitchBot
{
    internal class TwitchChatBot
    {
        readonly ConnectionCredentials credentials = new ConnectionCredentials(TwitchInfo.BotUserName, TwitchInfo.BotToken);
        TwitchClient client;
        List<string> reminderList = new List<string>();
        List<string> magicConchList = new List<string>();
        static Random rnd = new Random();
        int r;
        Timer t;
        String commands = "!reminder, !magicconch, !kermit, !bard, !hello";

        internal void Connect()
        {
            Console.WriteLine("Connecting...");
            client = new TwitchClient(credentials, TwitchInfo.ChannelName, '!', '!', logging: false);
            
            client.ChatThrottler = new TwitchLib.Services.MessageThrottler(20 / 2, TimeSpan.FromSeconds(30));
            client.WhisperThrottler = new TwitchLib.Services.MessageThrottler(20 / 2, TimeSpan.FromSeconds(30));

            client.OnConnectionError += Client_OnConnectionError;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;
            //client.OnBeingHosted += Client_OnBeingHosted;

            ConstructLists();
            t = new Timer(TimeSpan.FromMinutes(30).TotalMilliseconds);
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerElapsed);
            t.Start();

            client.Connect();
            client.SendMessage("Bartlebot has arrived!");

            TwitchAPI.Settings.ClientId = TwitchInfo.ClientId;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            client.SendMessage("Like to tweet? Follow me at @Barastos on Twitter! Interested in getting involved with the PvP community at large? Check out the Team Tryhard Discord channel @ https://pioneer2.co/.");
        }

        //when receiving a command, e.g. !help
        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            if (e.Command.Command == "help")
            {
                client.SendMessage("Here is a helpful list of commands you can use. [" + commands + "]");
            }

            //randomly choose a reminder from the list
            else if (e.Command.Command == "reminder")
            {
                r = rnd.Next(reminderList.Count);
                client.SendMessage((string)reminderList[r]);
            }

            //works similar to !reminder, randomly chooses a response from the Magic Conch Shell 
            else if (e.Command.Command == "magicconch")
            {
                r = rnd.Next(magicConchList.Count);
                client.SendMessage((string)magicConchList[r]);
            }

            else if (e.Command.Command == "bard")
            {
                client.SendMessage("No skill OSfrog No brain SabaPing I am DansGame a Bard main Kappa");
            }

            else if (e.Command.Command == "kermit")
            {
                client.SendMessage("OSfrog DON'T OSfrog FORGET OSfrog");
            }

            else if (e.Command.Command == "hello")
            {
                client.SendMessage("Heeeey HeyGuys");
            }

            else
            {
                client.SendMessage("That command is not recognized. For a complete list of commands, try !help");
            }
        }

        private void Client_OnBeingHosted(object sender, OnBeingHostedArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message == "commands")
            {
                //print out commands
                client.SendMessage("Here be a list of commands.");
            }
        }

        private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"error!! {e.Error}");
        }

        private void ConstructLists()
        {
            reminderList.Add("Daily reminder that Monk is overpowered and needs to be nerfed. OSfrog");
            reminderList.Add("Daily reminder that PvP is over-complicated and needs to be nerfed. OSfrog");
            reminderList.Add("Daily reminder that PvP is over-simplified and needs to be buffed. OSfrog");
            reminderList.Add("Daily reminder that you should be drinking about 2 liters of water every day. OSfrog");
            reminderList.Add("Daily reminder that Tanaka died for our sins and that Yoshida is merely piggy-backing off his creation. OSfrog");
            reminderList.Add("Daily reminder that friends don't let friends play Bard. OSfrog");
            reminderList.Add("Daily reminder that Uncle Acid win trades in LP and isn't ashamed of it. OSfrog");


            magicConchList.Add("Yes.");
            magicConchList.Add("No.");
            magicConchList.Add("Nothing.");
            magicConchList.Add("Maybe someday.");
            magicConchList.Add("Neither.");
            magicConchList.Add("Follow the seahorse.");
            magicConchList.Add("I don't think so.");
            magicConchList.Add("Try asking again.");
        }

        internal void Disconnect()
        {
            Console.WriteLine("Disconnecting...");
        }
    }
}