using Discord;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Discord.WebSocket;
using Ranko.Resources.Database;

namespace Ranko.Services
{
    public class RemindMeService
    {
        private const int timerInterval = 5 * 60 * 1000; // in miliseconds, 5minutes 
        private const int noAlertTimespan = 6 * 60 * 60 * 1000;//in miliseconds, time before deadline in which there wont be alerts anymore
        private DiscordSocketClient client { get; }
        private List<Resources.Database.TaskEntity> tasks;
        public RemindMeService(DiscordSocketClient _client)
        {
            client = _client;
            client.JoinedGuild += JoinedGuild;
            client.LeftGuild += LeftGuild;
            //check if there are some old underway tasks in database, process them
            tasks = SqliteDbHandler.GetAllTasks();
            List<GuildConfigEntity> guilds = SqliteDbHandler.GetAllGuilds();
            guilds.Except(guilds.Where(a => client.Guilds.Any(b=> b.Id == a.GuildId))).Select(x => x.GuildId).ToList().ForEach(y=> SqliteDbHandler.DeleteGuildConfig(y).GetAwaiter().GetResult());
            client.Guilds.Except(client.Guilds.Where(a => guilds.Any(b => b.GuildId == a.Id))).ToList().ForEach(c => JoinedGuild(c).GetAwaiter().GetResult());
            tasks.Where(x => x.DeadlineDate.CompareTo(DateTime.Now) <= 0).Where(x => x.CompletionStatus == 0).ToList().ForEach(x => //compare deadline time to current date of undergoing tasks
            {//deadline is over, task uncompleted
                SqliteDbHandler.SetCompletionStatus(x, 2).GetAwaiter().GetResult();
                IGuild guild = client.GetGuild(x.GuildId);
                if (guild != null)
                {
                    ITextChannel channel = (ITextChannel)guild.GetChannelAsync(x.Guild.AlertChannelId);
                    if(channel !=null)
                    {
                        IGuildUser user = (IGuildUser)guild.GetUserAsync(x.AssignedUserId);
                        if(user != null)
                        {
                            channel.SendMessageAsync($"Task \"{x.Name}: {x.Description}\" assigned to {user.Mention} was not completed on time({x.DeadlineDate.Date}).");
                        }
                    }
                }
            });
            new Timer(x => ReminderLoop(x), null, 0, timerInterval);
        }

        private async Task JoinedGuild(SocketGuild guild)
        {
            await guild.DefaultChannel.SendMessageAsync("test");
        }

        private async Task LeftGuild(SocketGuild guild)
        {
            //TODO: delete from database
        }
        private void ReminderLoop(Object stateInfo)
        {
            tasks = SqliteDbHandler.GetAllTasks();
            DateTime now = new DateTime(DateTime.Now.Ticks);
            tasks.Where(x => x.DeadlineDate.CompareTo(now) <= 0).Where(x => x.CompletionStatus == 0).ToList().ForEach(x =>
            {
                SqliteDbHandler.SetCompletionStatus(x, 2).GetAwaiter().GetResult();
                IGuild guild = client.GetGuild(x.GuildId);
                if (guild != null)
                {
                    ITextChannel channel = (ITextChannel)guild.GetChannelAsync(x.Guild.AlertChannelId);
                    if (channel != null)
                    {
                        IGuildUser user = (IGuildUser)guild.GetUserAsync(x.AssignedUserId);
                        if (user != null)
                        {
                            channel.SendMessageAsync($"Task \"{x.Name}: {x.Description}\" assigned to {user.Mention} was not completed on time({x.DeadlineDate.Date}).");
                        }
                    }
                }
            });
            
            tasks.Where(x => x.DeadlineDate.CompareTo(now) > 0).Where(x => x.CompletionStatus == 0).ToList().ForEach(x =>
            {
                //check if deadline time -last alert time/2 jest mniejszy niz teraz jesli tak to zmien i daj alert
                //ALERT SYSTEM
                DateTime newAlert = new DateTime(((x.DeadlineDate.Ticks - x.LastAlertDate.Ticks) / 2)+ x.LastAlertDate.Ticks);
                if(newAlert.CompareTo(now) <= 0)
                {
                    if((x.DeadlineDate.Ticks-now.Ticks/ TimeSpan.TicksPerMillisecond) > noAlertTimespan)
                    {
                        SqliteDbHandler.SetLastAlertDate(x.TaskId, newAlert).GetAwaiter().GetResult();
                        newAlert = new DateTime(((x.DeadlineDate.Ticks - newAlert.Ticks) / 2) +newAlert.Ticks);
                        if (newAlert.CompareTo(now) > 0)
                        {
                            IGuild guild = client.GetGuild(x.GuildId);
                            if (guild != null)
                            {
                                ITextChannel channel = (ITextChannel)guild.GetChannelAsync(x.Guild.AlertChannelId);
                                if (channel != null)
                                {
                                    IGuildUser user = (IGuildUser)guild.GetUserAsync(x.AssignedUserId);
                                    if (user != null)
                                    {
                                        channel.SendMessageAsync($"Task \"{x.Name}: {x.Description}\" assigned to {user.Mention} ends on {x.DeadlineDate.Date}.");
                                    }
                                }
                            }
                        }
                    }
                }

            });
        }

        internal async Task licz(IGuild guild, IMessageChannel channel)
        {

        }
        /*

        private static void ReminderLoop(Object stateInfo, IGuild guild)
        {
            Console.WriteLine($"interval in {guild.Name} time {DateTime.Now}");
        }

        internal async Task licz(IGuild guild, IMessageChannel channel)
        {
            
        }*/
    }
}
