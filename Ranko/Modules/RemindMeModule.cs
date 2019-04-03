using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ranko.Preconditions;
using Ranko.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Data.Sqlite;

namespace Ranko.Modules
{
    [RequireContext(ContextType.Guild)]
    public class RemindMeModule : ModuleBase<SocketCommandContext>
    {
        private readonly RemindMeService _service;

        protected RemindMeModule(RemindMeService service)
        {
            _service = service;
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        public virtual Task SetAdminRoles(params SocketRole[] roles)
        {
            try
            {
                return _service.SetAdminRoles(Context.Guild, roles);
            }
            catch (SqliteException e)
            {
                Context.Message.AddReactionAsync(new Emoji("❌"));
                return Task.FromException(e);
            }
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        public virtual Task AddAdminRoles(params SocketRole[] roles)
        {
            try
            {
                return _service.AddAdminRoles(Context.Guild, roles);
            }
            catch (SqliteException e)
            {
                Context.Message.AddReactionAsync(new Emoji("❌"));
                return Task.FromException(e);
            }
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        public virtual Task RemoveAdminRoles(params SocketRole[] roles)
        {
            try
            { 
                return _service.RemoveAdminRoles(Context.Guild, roles);
            }
            catch (SqliteException e)
            {
                Context.Message.AddReactionAsync(new Emoji("❌"));
                return Task.FromException(e);
            }
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        public virtual Task GetAdminRoles()
        {
            try
            { 
                return ReplyAsync($"Current administrator roles: {String.Join(", ", _service.GetAdminRoles(Context.Guild).Select(x => x.Name))}");
            }
            catch (SqliteException e)
            {
                Context.Message.AddReactionAsync(new Emoji("❌"));
                return Task.FromException(e);
            }
        }

        [RequireAdminRole]
        public virtual Task SetAlertChannel(SocketTextChannel channel)
        {
            try
            { 
                return _service.SetAlertChannel(Context.Guild, channel);
            }
            catch (SqliteException e)
            {
                Context.Message.AddReactionAsync(new Emoji("❌"));
                return Task.FromException(e);
            }
        }

        [RequireAdminRole]
        public virtual Task SetAlertChannel()
        {
            try
            { 
                return _service.SetAlertChannel(Context.Guild, Context.Channel);
            }
            catch (SqliteException e)
            {
                Context.Message.AddReactionAsync(new Emoji("❌"));
                return Task.FromException(e);
            }
        }

        [RequireAdminRole]
        public virtual Task GetAlertChannel()
        {
            try
            {
                return ReplyAsync($"Current channel for alerts: <#{_service.GetAlertChannel(Context.Guild).Id}>");
            }
            catch (SqliteException e)
            {
                Context.Message.AddReactionAsync(new Emoji("❌"));
                return Task.FromException(e);
            }
        }

        [RequireAdminRole]
        public virtual Task SetCommandChannel(SocketTextChannel channel)
        {
            try
            { 
                return _service.SetCommandChannel(Context.Guild, channel);
            }
            catch (SqliteException e)
            {
                Context.Message.AddReactionAsync(new Emoji("❌"));
                return Task.FromException(e);
            }
        }

        [RequireAdminRole]
        public virtual Task SetCommandChannel()
        {
            try
            { 
                return _service.SetCommandChannel(Context.Guild, Context.Channel);
            }
            catch (SqliteException e)
            {
                Context.Message.AddReactionAsync(new Emoji("❌"));
                return Task.FromException(e);
            }
        }

        [RequireAdminRole]
        public virtual Task GetCommandChannel()
        {
            try
            { 
                return ReplyAsync($"Current channel for commands: <#{_service.GetCommandChannel(Context.Guild).Id}>");
            }
            catch (SqliteException e)
            {
                Context.Message.AddReactionAsync(new Emoji("❌"));
                return Task.FromException(e);
            }
        }

        
        [RequireAdminRole]
        public virtual async Task AddTask(SocketUser user)
        {
            Embed embed = new EmbedBuilder()
            {
                Title = "Enter name of the task:"
            }.WithColor(Color.Gold)
            .Build();
            IUserMessage message = await ReplyAsync(embed: embed);
            await message.AddReactionAsync(new Emoji("❌"));
            string name = "", description = "";
            DateTime deadline;
            Func<Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> addedReactionHandler = null;
            Func<SocketMessage, Task> receivedMessageHandler = null;
            receivedMessageHandler = (m) =>
            {
                if (m.Author == Context.Message.Author)
                {
                    if (String.IsNullOrEmpty(name))
                    {
                        name = m.Content;
                        message.RemoveAllReactionsAsync();
                        message.ModifyAsync(msg => msg.Embed = message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.Green).Build());
                        embed = new EmbedBuilder()
                        {
                            Title = "Enter description of the task:"
                        }.WithColor(Color.Gold)
                        .Build();
                        message = ReplyAsync(embed: embed).GetAwaiter().GetResult();
                        message.AddReactionsAsync(new Emoji[] { new Emoji("🔙"), new Emoji("❌") });
                    }
                    else if (String.IsNullOrEmpty(description))
                    {
                        description = m.Content;
                        message.RemoveAllReactionsAsync();
                        message.ModifyAsync(msg => msg.Embed = message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.Green).Build());
                        embed = new EmbedBuilder()
                        {
                            Title = "Enter deadline of the task:"
                        }.WithColor(Color.Gold)
                        .Build();
                        message = ReplyAsync(embed: embed).GetAwaiter().GetResult();
                        message.AddReactionsAsync(new Emoji[] { new Emoji("🔙"), new Emoji("❌") });
                    }
                    else
                    {
                        if (DateTime.TryParse(m.Content, out deadline))
                        {
                            message.RemoveAllReactionsAsync();
                            message.ModifyAsync(msg => msg.Embed = message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.Green).Build());
                            try
                            { 
                                _service.AddTask(Context.Guild, Context.User, name, description, deadline).GetAwaiter().GetResult();
                            }
                            catch (SqliteException e)
                            {
                                Context.Message.AddReactionAsync(new Emoji("❌"));
                                UnsubscribeEvent(Context.Client, addedReactionHandler, receivedMessageHandler);
                                return Task.FromException(e);
                            }
                            embed = new EmbedBuilder()
                            {
                                Title = name,
                                Description = $"{ description}\n\nAssigned to { user.Username }\nDue to: { deadline.Date.ToString("d")}"
                            }.WithFooter(footer => footer.Text = "Added successfully")
                            .WithCurrentTimestamp()
                            .WithColor(Color.Green)
                            .Build();
                            message = ReplyAsync(embed: embed).GetAwaiter().GetResult();
                            UnsubscribeEvent(Context.Client, addedReactionHandler, receivedMessageHandler);
                        }
                        else
                        {
                            message.RemoveAllReactionsAsync();
                            message.ModifyAsync(msg => msg.Embed = message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.DarkRed).Build());
                            embed = new EmbedBuilder()
                            {
                                Title = "Invalid date format. Try again:"
                            }.WithColor(Color.Gold)
                            .Build();
                            message = ReplyAsync(embed: embed).GetAwaiter().GetResult();
                            message.AddReactionsAsync(new Emoji[] { new Emoji("🔙"), new Emoji("❌") });
                        }

                    }
                    
                }
                return Task.CompletedTask;
            };
            addedReactionHandler = (m, c, r) =>
            {
                if (m.HasValue && r.User.IsSpecified)
                {
                    if (m.Value.Id == message.Id && r.User.Value.Id == Context.Message.Author.Id)
                    {
                        if (r.Emote.Name.Equals(new Emoji("❌").Name))
                        {
                            message.RemoveAllReactionsAsync();
                            message.ModifyAsync(msg => msg.Embed = message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.DarkRed).Build());
                            UnsubscribeEvent(Context.Client, addedReactionHandler, receivedMessageHandler);
                        }
                        else if (r.Emote.Name.Equals(new Emoji("🔙").Name))
                        {
                            message.RemoveAllReactionsAsync();
                            if(String.IsNullOrEmpty(description))
                            {
                                name = "";
                                message.RemoveAllReactionsAsync();
                                message.ModifyAsync(msg => msg.Embed = message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.DarkRed).Build());
                                embed = new EmbedBuilder()
                                {
                                    Title = "Enter name of the task:"
                                }.WithColor(Color.Gold)
                                .Build();
                                message = ReplyAsync(embed: embed).GetAwaiter().GetResult();
                                message.AddReactionAsync(new Emoji("❌"));
                            }
                            else
                            {
                                description = "";
                                message.RemoveAllReactionsAsync();
                                message.ModifyAsync(msg => msg.Embed = message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.DarkRed).Build());
                                embed = new EmbedBuilder()
                                {
                                    Title = "Enter description of the task:"
                                }.WithColor(Color.Gold)
                                .Build();
                                message = ReplyAsync(embed: embed).GetAwaiter().GetResult();
                                message.AddReactionsAsync(new Emoji[] { new Emoji("🔙"), new Emoji("❌") });
                            }
                        }
                    }
                }
                return Task.CompletedTask;
            };
            Context.Client.ReactionAdded += addedReactionHandler;
            Context.Client.MessageReceived += receivedMessageHandler;
            new Timer(x => UnsubscribeEvent(Context.Client, addedReactionHandler, receivedMessageHandler), null, 90000, Timeout.Infinite);
        }

        private static void UnsubscribeEvent(DiscordSocketClient client, Func<Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> reactionHandler, Func<SocketMessage, Task> messageHandler)
        {
            client.ReactionAdded -= reactionHandler;
            client.MessageReceived -= messageHandler;
        }
    }
}
