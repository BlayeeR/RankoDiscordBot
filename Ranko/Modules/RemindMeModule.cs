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
            return _service.SetAdminRoles(Context.Guild, roles);
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        public virtual Task AddAdminRoles(params SocketRole[] roles)
        {
            return _service.AddAdminRoles(Context.Guild, roles);
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        public virtual Task RemoveAdminRoles(params SocketRole[] roles)
        {
            return _service.RemoveAdminRoles(Context.Guild, roles);
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        public virtual Task GetAdminRoles()
        {
            return ReplyAsync($"Current administrator roles: {String.Join(", ", _service.GetAdminRoles(Context.Guild).Select(x => x.Name))}");
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireAdminRole(Group = "Permission")]
        public virtual Task SetAlertChannel(SocketTextChannel channel)
        {
            return _service.SetAlertChannel(Context.Guild, channel);
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireAdminRole(Group = "Permission")]
        public virtual Task SetAlertChannel()
        {
            return _service.SetAlertChannel(Context.Guild, Context.Channel);
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireAdminRole(Group = "Permission")]
        public virtual Task GetAlertChannel()
        {
            return ReplyAsync($"Current channel for alerts: <#{_service.GetAlertChannel(Context.Guild).Id}>");
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireAdminRole(Group = "Permission")]
        public virtual Task SetCommandChannel(SocketTextChannel channel)
        {
            return _service.SetCommandChannel(Context.Guild, channel);
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireAdminRole(Group = "Permission")]
        public virtual Task SetCommandChannel()
        {
            return _service.SetCommandChannel(Context.Guild, Context.Channel);
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireAdminRole(Group = "Permission")]
        public virtual Task GetCommandChannel()
        {
            return ReplyAsync($"Current channel for commands: <#{_service.GetCommandChannel(Context.Guild).Id}>");
        }

    }
}
