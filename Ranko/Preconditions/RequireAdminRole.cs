using Discord.Commands;
using Discord;
using Ranko.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord.WebSocket;

namespace Ranko.Preconditions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal sealed class RequireAdminRole : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider map)
        {
            var service = map.GetService<RemindMeService>();
            if (service != null)
            {
                //if (CheckAllowCommands(service, context))
                //{

                return SqliteDbHandler.GetAdminRoles(context.Guild).Any(x => ((SocketGuildUser)context.User).Roles.Select(y=>y.Id).Contains(x))
                    ? Task.FromResult(PreconditionResult.FromSuccess())
                    : Task.FromResult(PreconditionResult.FromError("This command can only be used by user with admin role."));
                //}
                //return Task.FromResult(PreconditionResult.FromError("Managing music via commands is disabled in this guild."));
            }

            return Task.FromResult(PreconditionResult.FromError("No RemindMeService found."));
        }
    }
}
