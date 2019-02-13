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

        [Command("test", RunMode = RunMode.Async)]
        public override Task SetAdminRoles(params SocketRole[] roles)
        {
            if (roles == null)
            {
                return Task.CompletedTask;
            }
            else
            {
                ReplyAsync("abc");
                return base.SetAdminRoles(roles);
            }
        }

        [Command("test2", RunMode = RunMode.Async)]
        public override Task Test()
        {
            return ReplyAsync("abc");

        }
    }
}
