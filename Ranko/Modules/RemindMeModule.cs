using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ranko.Preconditions;
using Ranko.Services;
using System;
using System.Collections.Generic;
using System.Text;
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
            string text = String.Empty;
            foreach (SocketRole role in _service.GetAdminRoles(Context.Guild))
                text += $"{role.Name}, ";
            return ReplyAsync(text);
        }

        [RequireAdminRole(Group = "Permission")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        public virtual Task Test()
        {
            return _service.Test();
        }
    }
}
