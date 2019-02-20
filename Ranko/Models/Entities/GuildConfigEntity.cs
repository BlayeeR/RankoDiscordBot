using Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ranko.Resources.Database
{
    public class GuildConfigEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong GuildId { get; set; }
        public ulong CommandChannelId { get; set; }
        public ulong AlertChannelId { get; set; }
        public virtual List<AdminRoleEntity> AdminRoles { get; set; }
        public uint DeleteAlertMessageTimespan { get; set; }
        public ushort DateFormat { get; set; }
        public ushort Language { get; set; }
        public string DefaultTimezone { get; set; }
        public virtual List<TaskEntity> Tasks { get; set; }
    }
}
