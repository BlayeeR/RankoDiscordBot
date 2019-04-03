using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ranko.Modules;
using Ranko.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Ranko.ModuleImplementations
{
    public class RemindMeModuleImplementation : RemindMeModule
    {
        protected RemindMeModuleImplementation(RemindMeService service) : base(service)
        { }

        [Command("setadminroles", RunMode = RunMode.Async)]
        public override Task SetAdminRoles(params SocketRole[] roles)
        {
            if (roles != null)
            {
                base.SetAdminRoles(roles);
                return ReplyAsync($"Changed bot administrator roles to: {String.Join(", ", roles.Select(x => x.Name).ToArray())}");
            }
            else
                return Task.CompletedTask;
        }

        [Command("addadminroles", RunMode = RunMode.Async)]
        public override Task AddAdminRoles(params SocketRole[] roles)
        {
            if (roles != null)
            {
                base.AddAdminRoles(roles);
                return ReplyAsync($"Added selected roles as bot administrators: {String.Join(", ", roles.Select(x => x.Name).ToArray())}");
            }
            else
                return Task.CompletedTask;
        }


        [Command("removeadminroles", RunMode = RunMode.Async)]
        public override Task RemoveAdminRoles(params SocketRole[] roles)
        {
            if (roles != null)
            {
                base.RemoveAdminRoles(roles);
                return ReplyAsync($"Removed selected roles from bot administrators: {String.Join(", ", roles.Select(x => x.Name).ToArray())}");
            }
            else
                return Task.CompletedTask;
        }

        [Command("getadminroles", RunMode = RunMode.Async)]
        public override Task GetAdminRoles()
        {
            return base.GetAdminRoles();
        }

        [Command("setalertchannel", RunMode = RunMode.Async)]
        public override Task SetAlertChannel(SocketTextChannel channel)
        {
            base.SetAlertChannel(channel);
            return ReplyAsync($"<#{channel.Id}> is now set as channel for alerts");
        }

        [Command("setalertchannel", RunMode = RunMode.Async)]
        public override Task SetAlertChannel()
        {
            base.SetAlertChannel();
            return ReplyAsync($"<#{Context.Channel.Id}> is now set as channel for alerts");
        }

        [Command("getalertchannel", RunMode = RunMode.Async)]
        public override Task GetAlertChannel()
        {
            return base.GetAlertChannel();
        }

        [Command("setcommandchannel", RunMode = RunMode.Async)]
        public override Task SetCommandChannel(SocketTextChannel channel)
        {
            base.SetCommandChannel(channel);
            return ReplyAsync($"<#{channel.Id}> is now set as channel for commands");
        }

        [Command("setcommandchannel", RunMode = RunMode.Async)]
        public override Task SetCommandChannel()
        {
            base.SetCommandChannel();
            return ReplyAsync($"<#{Context.Channel.Id}> is now set as channel for commands");
        }

        [Command("getcommandchannel", RunMode = RunMode.Async)]
        public override Task GetCommandChannel()
        {
            return base.GetCommandChannel();
        }

        [Command("addtask", RunMode = RunMode.Async)]
        public override Task AddTask(SocketUser user)
        {
            return base.AddTask(user);
        }


    }
}
