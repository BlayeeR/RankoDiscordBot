using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ranko.Modules;
using Ranko.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
                ReplyAsync($"Added");
                return base.SetAdminRoles(roles);
            }
            else
                return Task.CompletedTask;
        }

        [Command("addadminroles", RunMode = RunMode.Async)]
        public override Task AddAdminRoles(params SocketRole[] roles)
        {
            if (roles != null)
            {
                ReplyAsync($"Added");
                return base.AddAdminRoles(roles);
            }
            else
                return Task.CompletedTask;
        }

        [Command("removeadminroles", RunMode = RunMode.Async)]
        public override Task RemoveAdminRoles(params SocketRole[] roles)
        {
            if (roles != null)
            {
                ReplyAsync($"Added");
                return base.RemoveAdminRoles(roles);
            }
            else
                return Task.CompletedTask;
        }

        [Command("getadminroles", RunMode = RunMode.Async)]
        public override Task GetAdminRoles()
        {
            return base.GetAdminRoles();
        }

        [Command("test2", RunMode = RunMode.Async)]
        public override Task Test()
        {
            return ReplyAsync("abc");

        }
    }
}
