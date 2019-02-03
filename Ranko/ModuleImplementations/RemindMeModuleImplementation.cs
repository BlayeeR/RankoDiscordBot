using Discord.Commands;
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
        public override Task licz()
        {
            return base.licz();
        }
    }
}
