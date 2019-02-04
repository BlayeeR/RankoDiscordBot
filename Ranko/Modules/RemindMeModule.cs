using Discord;
using Discord.Commands;
using Discord.WebSocket;
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

        public virtual Task SetAdminRoles(params SocketRole[] roles)
        {
            return _service.SetAdminRoles(Context.Guild, roles);
        }

    }
}
