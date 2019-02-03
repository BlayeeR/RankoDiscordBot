using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Ranko.Services;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Ranko
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private RemindMeService _remindMeService;
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            string botToken = "";
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _remindMeService = new RemindMeService(_client);
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(_remindMeService)
                .BuildServiceProvider();

            _client.Log += Log;
            _client.Ready += Ready;

            await RegisterCommandsAsync();

            using (var Stream = new FileStream(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.1", @"Data\Token.txt"), FileMode.Open, FileAccess.Read))
            using (var ReadToken = new StreamReader(Stream))
            {
                botToken = ReadToken.ReadToEnd();
            }
            await _client.LoginAsync(Discord.TokenType.Bot, botToken);

            await _client.StartAsync();

            await Task.Delay(-1);
        }


        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

            int argPos = 0;

            if (message.HasStringPrefix("r.", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);

            }
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            /*if (_client.ConnectionState == ConnectionState.Connected && !arg.Source.Equals("Rest"))
            {
                try
                {
                    var server = _client.Guilds.FirstOrDefault(x => x.Id == 344624539562541056);
                    var channel = server.Channels.First(x => x.Id == 428634381305905163) as ITextChannel;
                    channel.SendMessageAsync($"[{DateTime.Now.ToString("HH:mm:s")}] {arg.Source}: {arg.Message}");
                }
                catch
                {
                    return Task.CompletedTask;
                }
            }*/
            return Task.CompletedTask;
        }

        private async Task Ready()
        {
            await _client.SetGameAsync("B-BAKA! Don't touch my code KYAA >///< OwO");
        }

    }
}
