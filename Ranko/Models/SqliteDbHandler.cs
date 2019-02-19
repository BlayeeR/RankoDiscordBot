using Ranko.Resources.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using System.Threading.Tasks;

namespace Ranko
{
    public static class SqliteDbHandler
    {

        public static async Task RemoveGuildConfig(IGuild guild)
        {
            await RemoveGuildConfig(guild.Id);
        }
        public static async Task RemoveGuildConfig(ulong guildId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                DbContext.GuildConfig.Remove(DbContext.GuildConfig.Where(x => x.GuildId == guildId).First());
                await DbContext.SaveChangesAsync();
            }
        }

        public static List<Resources.Database.TaskEntity> GetTasks(IGuild guild)
        {
            return GetTasks(guild.Id);
        }
        public static List<Resources.Database.TaskEntity> GetTasks(ulong guildId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                return DbContext.Tasks.Where(x => x.GuildId == guildId).ToList();
            }
        }
        public static ulong GetCommandChannelId(IGuild guild)
        {
            return GetCommandChannelId(guild.Id);
        }
        public static ulong GetCommandChannelId(ulong guildId)//gets channel in specified guild where you can use bot commands, 0=any
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                return DbContext.GuildConfig.Where(x => x.GuildId == guildId).Select(x => x.CommandChannelId).FirstOrDefault();//0=not set

            }
        }

        public static async Task SetCommandChannelId(IGuild guild, IMessageChannel commandChannel)
        {
            await SetCommandChannelId(guild.Id, commandChannel.Id);
        }
        public static async Task SetCommandChannelId(IGuild guild, ulong commandChannelId)
        {
            await SetCommandChannelId(guild.Id, commandChannelId);
        }
        public static async Task SetCommandChannelId(ulong guildId, IMessageChannel commandChannel)
        {
            await SetCommandChannelId(guildId, commandChannel.Id);
        }
        public static async Task SetCommandChannelId(ulong guildId, ulong commandChannelId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().CommandChannelId = commandChannelId;
                await DbContext.SaveChangesAsync();
            }
        }

        public static ulong GetAlertChannelId(IGuild guild)
        {
            return GetAlertChannelId(guild.Id);
        }
        public static ulong GetAlertChannelId(ulong guildId)//gets channel in specified where bot alerts users 
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                return DbContext.GuildConfig.Where(x => x.GuildId == guildId).Select(x => x.AlertChannelId).FirstOrDefault(); //0=not set
            }
        }
        public static async Task SetAlertChannelId(IGuild guild, IMessageChannel alertChannel)
        {
            await SetAlertChannelId(guild.Id, alertChannel.Id);
        }
        public static async Task SetAlertChannelId(IGuild guild, ulong alertChannelId)
        {
            await SetAlertChannelId(guild.Id, alertChannelId);
        }
        public static async Task SetAlertChannelId(ulong guildId, IMessageChannel alertChannel)
        {
            await SetAlertChannelId(guildId, alertChannel.Id);
        }
        public static async Task SetAlertChannelId(ulong guildId, ulong alertChannelId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().AlertChannelId = alertChannelId;
                await DbContext.SaveChangesAsync();
            }
        }

        public static uint GetDeleteAlertMessageTimespan(IGuild guild)
        {
            return GetDeleteAlertMessageTimespan(guild.Id);
        }
        public static uint GetDeleteAlertMessageTimespan(ulong guildId)//gets time interval after which alert message gets deleted(its get deleted automatically if the time is later than next ping or task end(not more than 2 weeks- discord limits)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                return DbContext.GuildConfig.Where(x => x.GuildId == guildId).Select(x => x.DeleteAlertMessageTimespan).FirstOrDefault(); //0=not set
            }
        }

        public static async Task SetDeleteAlertMesssageTimespan(IGuild guild, uint timespan)
        {
            await SetDeleteAlertMesssageTimespan(guild.Id, timespan);
        }
        public static async Task SetDeleteAlertMesssageTimespan(ulong guildId, uint timespan)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().DeleteAlertMessageTimespan = timespan;
                await DbContext.SaveChangesAsync();
            }
        }

        public static ushort GetDateFormat(IGuild guild)
        {
            return GetDateFormat(guild.Id);
        }
        public static ushort GetDateFormat(ulong guildId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                return DbContext.GuildConfig.Where(x => x.GuildId == guildId).Select(x => x.DateFormat).FirstOrDefault(); //0=int
            }
        }

        public static async Task SetDateFormat(IGuild guild, ushort dateFormatId)
        {
            await SetDateFormat(guild.Id, dateFormatId);
        }
        public static async Task SetDateFormat(ulong guildId, ushort dateFormatId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().DateFormat = dateFormatId;
                await DbContext.SaveChangesAsync();
            }
        }

        public static ushort GetLanguageId(IGuild guild)
        {
            return GetLanguageId(guild.Id);
        }
        public static ushort GetLanguageId(ulong guildId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                return DbContext.GuildConfig.Where(x => x.GuildId == guildId).Select(x => x.Language).FirstOrDefault(); //0=int
            }
        }

        public static async Task SetLanguageId(IGuild guild, ushort languageId)
        {
            await SetLanguageId(guild.Id, languageId);
        }
        public static async Task SetLanguageId(ulong guildId, ushort languageId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().Language = languageId;
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SetAdminRoles(IGuild guild, IRole[] roles)
        {
            await SetAdminRoles(guild.Id, roles.Select(x=>x.Id).ToArray());
        }
        public static async Task SetAdminRoles(IGuild guild, ulong[] rolesId)
        {
            await SetAdminRoles(guild.Id, rolesId);
        }
        public static async Task SetAdminRoles(ulong guildId, IRole[] roles)
        {
            await SetAdminRoles(guildId, roles.Select(x=>x.Id).ToArray());
        }
        public static async Task SetAdminRoles(ulong guildId, ulong[] roles)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }

                List<AdminRoleEntity> adminRoles = new List<AdminRoleEntity>();
                foreach (ulong role in roles)
                    adminRoles.Add(new AdminRoleEntity()
                    {
                        Guild = DbContext.GuildConfig.Where(y => y.GuildId == guildId).FirstOrDefault(),
                        GuildId = DbContext.GuildConfig.Where(y => y.GuildId == guildId).FirstOrDefault().GuildId,
                        RoleId = role
                    });

                DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().AdminRoles= adminRoles;
                await DbContext.SaveChangesAsync();
            }
        }
        public static async Task AddAdminRoles(IGuild guild, IRole[] roles)
        {
            await AddAdminRoles(guild.Id, roles.Select(x => x.Id).ToArray());
        }
        public static async Task AddAdminRoles(IGuild guild, ulong[] rolesId)
        {
            await AddAdminRoles(guild.Id, rolesId);
        }
        public static async Task AddAdminRoles(ulong guildId, IRole[] roles)
        {
            await AddAdminRoles(guildId, roles.Select(x => x.Id).ToArray());
        }
        public static async Task AddAdminRoles(ulong guildId, ulong[] rolesId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }

                List<AdminRoleEntity> adminRoles = new List<AdminRoleEntity>();
                foreach (ulong role in rolesId)
                    adminRoles.Add(new AdminRoleEntity()
                    {
                        Guild = DbContext.GuildConfig.Where(y => y.GuildId == guildId).FirstOrDefault(),
                        GuildId = DbContext.GuildConfig.Where(y => y.GuildId == guildId).FirstOrDefault().GuildId,
                        RoleId = role
                    });

                DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().AdminRoles.AddRange(adminRoles);
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task RemoveAdminRoles(IGuild guild, IRole[] roles)
        {
            await RemoveAdminRoles(guild.Id, roles.Select(x => x.Id).ToArray());
        }
        public static async Task RemoveAdminRoles(ulong guildId, IRole[] roles)
        {
            await RemoveAdminRoles(guildId, roles.Select(x => x.Id).ToArray());
        }
        public static async Task RemoveAdminRoles(IGuild guild, ulong[] rolesId)
        {
            await RemoveAdminRoles(guild.Id, rolesId);
        }

        public static async Task RemoveAdminRoles(ulong guildId, ulong[] rolesId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    return;
                }

                List<AdminRoleEntity> adminRoles = DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().AdminRoles;
                foreach (ulong role in rolesId)
                    adminRoles.Remove(adminRoles.Where(x => x.RoleId == role).FirstOrDefault());

                //test
                //DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().AdminRoles = adminRoles;
                await DbContext.SaveChangesAsync();
            }
        }

        public static List<ulong> GetAdminRoles(IGuild guild)
        {
            return GetAdminRoles(guild.Id);
        }

        public static List<ulong> GetAdminRoles(ulong guildId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                {
                    CreateDefaultGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                }
                List<AdminRoleEntity> admins = DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().AdminRoles;
                return admins.Select(x=> x.RoleId).ToList();
            }
        }

        private static async Task CreateDefaultGuildConfig(IGuild guild)
        {
            await CreateDefaultGuildConfig(guild.Id);
        }
        private static async Task CreateDefaultGuildConfig(ulong guildId)//creates default guild config
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() >= 1)
                    return;
                else
                {
                    DbContext.GuildConfig.Add(new GuildConfigEntity
                    {
                        GuildId = guildId,
                        CommandChannelId = 0,
                        AlertChannelId = 0,
                        AdminRoles = new List<AdminRoleEntity>(),
                        Tasks = new List<Resources.Database.TaskEntity>(),
                        DeleteAlertMessageTimespan = 604800,
                        DateFormat = 0,
                        Language = 0
                    });
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task CreateTask(ulong guildId, ulong userId, DateTime lastAlert, DateTime deadline)//TODO:ALERTINTERVAL 
        {
            using (var DbContext = new SqliteDbContext())
            {
                DbContext.Tasks.Add(new Resources.Database.TaskEntity()
                {
                    GuildId = guildId,
                    AssignedUserId = userId,
                    LastAlertDate = lastAlert,
                    DeadlineDate = deadline
                });
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task CreateTask(Resources.Database.TaskEntity task)
        {
            await CreateTask(task.GuildId, task.AssignedUserId, task.LastAlertDate, task.DeadlineDate);
        }

        public static async Task RemoveTask(ulong taskId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                    return;
                DbContext.Tasks.Remove(DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault());
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task RemoveTask(Resources.Database.TaskEntity task)
        {
            await RemoveTask(task.TaskId);
        }

        public static ulong GetAssignedUserId(Resources.Database.TaskEntity task)
        {
            return GetAssignedUserId(task.TaskId);

        }
        public static ulong GetAssignedUserId(ulong taskId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                    return 0;
                else
                    return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().AssignedUserId;
            }
        }

        public static List<Resources.Database.TaskEntity> GetUserTasks(IUser user)
        {
            return GetUserTasks(user.Id);

        }

        public static List<Resources.Database.TaskEntity> GetUserTasks(ulong userId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                return DbContext.Tasks.Where(x => x.AssignedUserId == userId).ToList();
            }
        }

        public static DateTime GetDeadline(Resources.Database.TaskEntity task)
        {
            return GetDeadline(task.TaskId);
        }
        public static DateTime GetDeadline(ulong taskId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                    return DateTime.MinValue;//alert doesnt exists 
                else
                    return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().DeadlineDate;
            }
        }

        public static DateTime GetLastAlertDate(Resources.Database.TaskEntity task)
        {
            return GetLastAlertDate(task.TaskId);
        }
        public static DateTime GetLastAlertDate(ulong taskId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                    return DateTime.MinValue;//alert doesnt exists 
                else
                    return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().LastAlertDate;
            }
        }

        public static async Task SetLastAlertDate(Resources.Database.TaskEntity task, DateTime lastAlert)
        {
            await SetLastAlertDate(task.TaskId, lastAlert);
        }
        public static async Task SetLastAlertDate(ulong taskId, DateTime lastAlert)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                    return;
                DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().LastAlertDate = lastAlert;
                await DbContext.SaveChangesAsync();
            }
        }

        public static ulong GetGuildId(Resources.Database.TaskEntity task)
        {
            return GetGuildId(task.TaskId);
        }

        public static ulong GetGuildId(ulong taskId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                    return 0;
                else
                    return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().GuildId;
            }
        }
        public static bool IsCompleted(Resources.Database.TaskEntity task)
        {
            return IsCompleted(task.TaskId);
        }
        public static bool IsCompleted(ulong taskId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                try
                {
                    return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().CompletionStatus == 1 ? true : false;
                }
                catch
                {
                    return false;
                }
            }

        }
        public static uint GetCompletionStatus(Resources.Database.TaskEntity task)
        {
            return GetCompletionStatus(task.TaskId);
        }
        public static uint GetCompletionStatus(ulong taskId)
        {
            if (!IsCompleted(taskId))
                return 0;
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                    return 0;
                else
                    return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().CompletionStatus;
            }
        }
        public static async Task SetCompletionStatus(Resources.Database.TaskEntity task, uint completionStatus)
        {
            await SetCompletionStatus(task.TaskId, completionStatus);
        }
        public static async Task SetCompletionStatus(ulong taskId, uint completionStatus)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                    return;
                DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().CompletionStatus = completionStatus;
                await DbContext.SaveChangesAsync();
            }
        }
        public static List<Resources.Database.TaskEntity> GetAllTasks()
        {
            using (var DbContext = new SqliteDbContext())
            {
                return DbContext.Tasks.ToList();
            }
        }

        public static async Task DeleteGuildConfig(IGuild guild)
        {
            await DeleteGuildConfig(guild.Id);
        }

        public static async Task DeleteGuildConfig(ulong guildId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                    return;
                DbContext.GuildConfig.Remove(DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault());
                await DbContext.SaveChangesAsync();
            }
        }
        public static List<GuildConfigEntity> GetAllGuilds()
        {
            using (var DbContext = new SqliteDbContext())
            {
                return DbContext.GuildConfig.ToList();
            }
        }
    }
}
