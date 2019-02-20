using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ranko.Resources.Database
{
    public class TaskEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong TaskId{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual GuildConfigEntity Guild { get; set; }
        public ulong GuildId { get; set; }
        public ulong AssignedUserId { get; set; }
        public DateTime LastAlertDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string TimeZone { get; set; }
        public uint CompletionStatus { get; set; } // 0underway, 1completed, 2not completed, 3canceled,
    }
}
