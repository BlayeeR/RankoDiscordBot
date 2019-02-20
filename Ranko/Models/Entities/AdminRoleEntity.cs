using Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ranko.Resources.Database
{
    public class AdminRoleEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong RoleId { get; set; }
        public virtual GuildConfigEntity Guild { get; set; }
        public ulong GuildId { get; set; }
    }
}
