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
        private DiscordSocketClient Client { get; }
        private List<Resources.Database.TaskEntity> tasks;
        public RemindMeService(DiscordSocketClient _client)
        {
            Client = _client;
            Client.JoinedGuild += JoinedGuild;
            Client.LeftGuild += LeftGuild;
            Client.Ready += Client_Ready;
        }

        private Task Client_Ready()
        {
            List<GuildConfigEntity> guilds = SqliteDbHandler.GetAllGuilds();

            //delete old guilds from database(guilds that bot is no longer in)
            guilds.Except(guilds.Where(a => Client.Guilds.Any(b => b.Id == a.GuildId))).Select(x => x.GuildId).ToList().ForEach(y => SqliteDbHandler.DeleteGuildConfig(y).GetAwaiter().GetResult());

            //create record in database for guilds that bot joined while being offline
            Client.Guilds.Except(Client.Guilds.Where(a => guilds.Any(b => b.GuildId == a.Id))).ToList().ForEach(c => JoinedGuild(c).GetAwaiter().GetResult());
            tasks = SqliteDbHandler.GetAllTasks();

            //check if there are some old underway tasks in database, process them
            //TODO: optimalization- pull every guild and user once from databse instead of doing it in every iteration
            List<TaskEntity> uncompletedTasks = tasks.Where(x => x.DeadlineDate.CompareTo(DateTime.Now) <= 0).Where(x => x.CompletionStatus == 0).ToList();//deadline is over, task uncompleted
            uncompletedTasks.Select(x => new { x.GuildId, x.AssignedUserId }).Distinct().ToList().ForEach(y => {
                IGuild guild = Client.GetGuild(y.GuildId);
                if (guild != null)
                {
                    SocketUser user = Client.GetUser(y.AssignedUserId);
                    if (user != null)
                    {
                        EmbedBuilder builder = new EmbedBuilder();
                        uncompletedTasks.Where(x => x.AssignedUserId == user.Id && x.GuildId == guild.Id).ToList().ForEach(z => {
                            SqliteDbHandler.SetCompletionStatus(z, 2).GetAwaiter().GetResult();
                            builder.AddField(new EmbedFieldBuilder()
                            {
                                Name = z.Name,
                                Value = $"{z.Description}\nDue to {z.DeadlineDate.Date.ToString("d")}\n"
                            });
                        });
                        builder.Color = Color.DarkRed;
                        GetAlertChannel(guild).SendMessageAsync($"{user.Mention} your tasks weren't completed on time:", embed: builder.Build());
                    }
                }
            });

            //start main loop
            new Timer(x => ReminderLoop(x), null, 0, timerInterval);
            return Task.CompletedTask;
        }

        private async Task JoinedGuild(SocketGuild guild)
        {
            await SqliteDbHandler.CreateDefaultGuildConfig(guild);
        }

        private async Task LeftGuild(SocketGuild guild)
        {
            await SqliteDbHandler.DeleteGuildConfig(guild);
        }

        private void ReminderLoop(Object stateInfo)
        {
            //tasks = SqliteDbHandler.GetAllTasks();
            //DateTime now = new DateTime(DateTime.Now.Ticks);

            ////process tasks which deadline is over
            ////get tasks with deadline date ealier than current date
            //List<TaskEntity> doneTasks = tasks.Where(x => x.DeadlineDate.CompareTo(now) <= 0).Where(x => x.CompletionStatus == 0).ToList();
            //doneTasks.ForEach(x =>
            //{
            //    //set task status as not completed
            //    SqliteDbHandler.SetCompletionStatus(x, 2).GetAwaiter().GetResult();
            //    IGuild guild = Client.GetGuild(x.GuildId);
            //    if (guild != null)
            //    {
            //        ITextChannel channel = (ITextChannel)guild.GetChannelAsync(x.Guild.AlertChannelId);
            //        if (channel != null)
            //        {
            //            IGuildUser user = (IGuildUser)guild.GetUserAsync(x.AssignedUserId);
            //            if (user != null)
            //            {
            //                //send message to guild that task is not completed on time
            //                channel.SendMessageAsync($"Task \"{x.Name}: {x.Description}\" assigned to {user.Mention} was not completed on time({x.DeadlineDate.Date}).");
            //            }
            //        }
            //    }
            //});
            
            ////update undergoing tasks
            ////tasks.Where(x => x.DeadlineDate.CompareTo(now) > 0).Where(x => x.CompletionStatus == 0).ToList().ForEach(x =>
            //tasks.Except(doneTasks).ToList().ForEach(x =>
            //{
            //    //check if deadline time -last alert time/2 is smaller than current time. if it is, update last alert time and create alert
            //    //ALERT SYSTEM
            //    DateTime newAlert = new DateTime(((x.DeadlineDate.Ticks - x.LastAlertDate.Ticks) / 2)+ x.LastAlertDate.Ticks);
            //    if(newAlert.CompareTo(now) <= 0)
            //    {
            //        if((x.DeadlineDate.Ticks-now.Ticks/ TimeSpan.TicksPerMillisecond) > noAlertTimespan)
            //        {
            //            SqliteDbHandler.SetLastAlertDate(x.TaskId, newAlert).GetAwaiter().GetResult();
            //            newAlert = new DateTime(((x.DeadlineDate.Ticks - newAlert.Ticks) / 2) +newAlert.Ticks);
            //            if (newAlert.CompareTo(now) > 0)
            //            {
            //                IGuild guild = Client.GetGuild(x.GuildId);
            //                if (guild != null)
            //                {
            //                    ITextChannel channel = (ITextChannel)guild.GetChannelAsync(x.Guild.AlertChannelId);
            //                    if (channel != null)
            //                    {
            //                        IGuildUser user = (IGuildUser)guild.GetUserAsync(x.AssignedUserId);
            //                        if (user != null)
            //                        {
            //                            channel.SendMessageAsync($"Task \"{x.Name}: {x.Description}\" assigned to {user.Mention} ends on {x.DeadlineDate.Date}.");
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //});
        }

        internal async Task SetAdminRoles(IGuild guild, IRole[] roles)
        {
            await SqliteDbHandler.SetAdminRoles(guild, roles);
        }

        internal async Task AddAdminRoles(IGuild guild, IRole[] roles)
        {
            await SqliteDbHandler.AddAdminRoles(guild, roles);
        }

        internal async Task RemoveAdminRoles(IGuild guild, IRole[] roles)
        {
            await SqliteDbHandler.RemoveAdminRoles(guild, roles);
        }

        internal List<SocketRole> GetAdminRoles(IGuild guild)
        {
            List<SocketRole> roles = new List<SocketRole>();
            foreach (ulong roleId in SqliteDbHandler.GetAdminRoles(guild))
                roles.Add(guild.Roles.Where(x => x.Id == roleId).FirstOrDefault() as SocketRole);
            return roles;
        }

        internal async Task SetAlertChannel(IGuild guild, IMessageChannel channel)
        {
            await SqliteDbHandler.SetAlertChannelId(guild, channel);
        }

        internal IMessageChannel GetAlertChannel(IGuild guild)
        {
            SocketTextChannel channel = guild.GetChannelAsync(SqliteDbHandler.GetAlertChannelId(guild)).GetAwaiter().GetResult() as SocketTextChannel;
            return channel != null? channel : GetCommandChannel(guild);
            //return guild.GetDefaultChannelAsync().GetAwaiter().GetResult() as SocketChannel;
        }

        internal async Task SetCommandChannel(IGuild guild, IMessageChannel channel)
        {
            await SqliteDbHandler.SetCommandChannelId(guild, channel);
        }

        internal IMessageChannel GetCommandChannel(IGuild guild)
        {
            SocketTextChannel channel = guild.GetChannelAsync(SqliteDbHandler.GetCommandChannelId(guild)).GetAwaiter().GetResult() as SocketTextChannel;
            return channel!=null?channel:guild.GetDefaultChannelAsync().GetAwaiter().GetResult();
        }

        internal async Task AddTask(IGuild guild, IUser assignedUser, string name, string description, DateTime deadline)
        {
            await SqliteDbHandler.CreateTask(guild.Id, assignedUser.Id, name, description, DateTime.Now, deadline);
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
