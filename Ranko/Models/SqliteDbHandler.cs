using Ranko.Resources.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                using (var DbContext = new SqliteDbContext())
                {
                    DbContext.GuildConfig.Remove(DbContext.GuildConfig.Where(x => x.GuildId == guildId).First());
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }
        public static List<Resources.Database.TaskEntity> GetTasks(IGuild guild)
        {
            return GetTasks(guild.Id);
        }
        public static List<Resources.Database.TaskEntity> GetTasks(ulong guildId)
        {
            try
            {
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    return DbContext.Tasks.Where(x => x.GuildId == guildId).ToList();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }
        public static ulong GetCommandChannelId(IGuild guild)
        {
            return GetCommandChannelId(guild.Id);
        }
        public static ulong GetCommandChannelId(ulong guildId)//gets channel in specified guild where you can use bot commands, 0=any
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    return DbContext.GuildConfig.Where(x => x.GuildId == guildId).Select(x => x.CommandChannelId).FirstOrDefault();//0=not set

                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
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
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().CommandChannelId = commandChannelId;
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static ulong GetAlertChannelId(IGuild guild)
        {
            return GetAlertChannelId(guild.Id);
        }
        public static ulong GetAlertChannelId(ulong guildId)//gets channel in specified where bot alerts users 
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    return DbContext.GuildConfig.Where(x => x.GuildId == guildId).Select(x => x.AlertChannelId).FirstOrDefault(); //0=not set
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
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
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().AlertChannelId = alertChannelId;
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static ushort GetDateFormat(IGuild guild)
        {
            return GetDateFormat(guild.Id);
        }
        public static ushort GetDateFormat(ulong guildId)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    return DbContext.GuildConfig.Where(x => x.GuildId == guildId).Select(x => x.DateFormat).FirstOrDefault(); //0=int
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static async Task SetDateFormat(IGuild guild, ushort dateFormatId)
        {
            await SetDateFormat(guild.Id, dateFormatId);
        }
        public static async Task SetDateFormat(ulong guildId, ushort dateFormatId)
        {
            try
            {
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().DateFormat = dateFormatId;
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static ushort GetLanguageId(IGuild guild)
        {
            return GetLanguageId(guild.Id);
        }
        public static ushort GetLanguageId(ulong guildId)
        {
            try
            {
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    return DbContext.GuildConfig.Where(x => x.GuildId == guildId).Select(x => x.Language).FirstOrDefault(); //0=int
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static async Task SetLanguageId(IGuild guild, ushort languageId)
        {
            await SetLanguageId(guild.Id, languageId);
        }
        public static async Task SetLanguageId(ulong guildId, ushort languageId)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().Language = languageId;
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static async Task SetAdminRoles(IGuild guild, IRole[] roles)
        {
            await SetAdminRoles(guild.Id, roles.Select(x=>x.Id).ToArray());
        }
        public static async Task SetAdminRoles(IGuild guild, ulong[] roleIds)
        {
            await SetAdminRoles(guild.Id, roleIds);
        }
        public static async Task SetAdminRoles(ulong guildId, IRole[] roles)
        {
            await SetAdminRoles(guildId, roles.Select(x=>x.Id).ToArray());
        }
        public static async Task SetAdminRoles(ulong guildId, ulong[] roleIds)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    GuildConfigEntity guild = DbContext.GuildConfig.Where(y => y.GuildId == guildId).FirstOrDefault();
                    DbContext.AdminRoles.RemoveRange(guild.AdminRoles);
                    foreach (ulong role in roleIds)
                        DbContext.AdminRoles.Add(new AdminRoleEntity()
                        {
                            GuildId = guild.GuildId,
                            RoleId = role
                        });
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }
        public static async Task AddAdminRoles(IGuild guild, IRole[] roles)
        {
            await AddAdminRoles(guild.Id, roles.Select(x => x.Id).ToArray());
        }
        public static async Task AddAdminRoles(IGuild guild, ulong[] roleIds)
        {
            await AddAdminRoles(guild.Id, roleIds);
        }
        public static async Task AddAdminRoles(ulong guildId, IRole[] roles)
        {
            await AddAdminRoles(guildId, roles.Select(x => x.Id).ToArray());
        }
        public static async Task AddAdminRoles(ulong guildId, ulong[] roleIds)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    foreach (ulong role in roleIds)
                        DbContext.AdminRoles.Add(new AdminRoleEntity()
                        {
                            GuildId = DbContext.GuildConfig.Where(y => y.GuildId == guildId).FirstOrDefault().GuildId,
                            RoleId = role
                        });
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
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
        public static async Task RemoveAdminRoles(IGuild guild, ulong[] roleIds)
        {
            await RemoveAdminRoles(guild.Id, roleIds);
        }

        public static async Task RemoveAdminRoles(ulong guildId, ulong[] roleIds)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                    {
                        CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                        return;
                    }
                    GuildConfigEntity guild = DbContext.GuildConfig.Where(y => y.GuildId == guildId).FirstOrDefault();
                    DbContext.AdminRoles.RemoveRange(guild.AdminRoles.Where(x => roleIds.Any(z => z == x.RoleId)));
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static List<ulong> GetAdminRoles(IGuild guild)
        {
            return GetAdminRoles(guild.Id);
        }

        public static List<ulong> GetAdminRoles(ulong guildId)
        {
            try
            {
                using (var DbContext = new SqliteDbContext())
                {
                    CreateGuildConfig(guildId).GetAwaiter().GetResult();//guild config not found
                    List<AdminRoleEntity> admins = DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault().AdminRoles;
                    return admins.Select(x => x.RoleId).ToList();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static async Task CreateDefaultGuildConfig(IGuild guild)
        {
            await CreateGuildConfig(guild.Id);
        }

        private static async Task CreateGuildConfig(ulong guildId)//creates default guild config
        {
            try
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
                            //DeleteAlertMessageTimespan = 604800,
                            DateFormat = 0,
                            Language = 0
                        });
                    }
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static async Task CreateTask(ulong guildId, ulong userId, string name, string description, DateTime lastAlertDate, DateTime deadline)//TODO:ALERTINTERVAL 
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    DbContext.Tasks.Add(new Resources.Database.TaskEntity()
                    {
                        GuildId = guildId,
                        AssignedUserId = userId,
                        Name = name,
                        Description = description,
                        LastAlertDate = lastAlertDate,
                        DeadlineDate = deadline
                    });
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static async Task CreateTask(Resources.Database.TaskEntity task)
        {
            await CreateTask(task.GuildId, task.AssignedUserId, task.Name, task.Description, task.LastAlertDate, task.DeadlineDate);
        }

        public static async Task RemoveTask(ulong taskId)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                        return;
                    DbContext.Tasks.Remove(DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault());
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
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
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                        return 0;
                    else
                        return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().AssignedUserId;
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static List<Resources.Database.TaskEntity> GetUserTasks(IUser user)
        {
            return GetUserTasks(user.Id);
        }

        public static List<Resources.Database.TaskEntity> GetUserTasks(ulong userId)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    return DbContext.Tasks.Where(x => x.AssignedUserId == userId).ToList();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static DateTime GetDeadline(Resources.Database.TaskEntity task)
        {
            return GetDeadline(task.TaskId);
        }
        public static DateTime GetDeadline(ulong taskId)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                        return DateTime.MinValue;//alert doesnt exists 
                    else
                        return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().DeadlineDate;
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static DateTime GetLastAlertDate(Resources.Database.TaskEntity task)
        {
            return GetLastAlertDate(task.TaskId);
        }
        public static DateTime GetLastAlertDate(ulong taskId)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                        return DateTime.MinValue;//alert doesnt exists 
                    else
                        return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().LastAlertDate;
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static async Task SetLastAlertDate(Resources.Database.TaskEntity task, DateTime lastAlert)
        {
            await SetLastAlertDate(task.TaskId, lastAlert);
        }
        public static async Task SetLastAlertDate(ulong taskId, DateTime lastAlert)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                        return;
                    DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().LastAlertDate = lastAlert;
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static ulong GetGuildId(Resources.Database.TaskEntity task)
        {
            return GetGuildId(task.TaskId);
        }

        public static ulong GetGuildId(ulong taskId)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                        return 0;
                    else
                        return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().GuildId;
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
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
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                        return 0;
                    else
                        return DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().CompletionStatus;
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }
        public static async Task SetCompletionStatus(Resources.Database.TaskEntity task, uint completionStatus)
        {
            await SetCompletionStatus(task.TaskId, completionStatus);
        }
        public static async Task SetCompletionStatus(ulong taskId, uint completionStatus)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    if (DbContext.Tasks.Where(x => x.TaskId == taskId).Count() < 1)
                        return;
                    DbContext.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault().CompletionStatus = completionStatus;
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }
        public static List<Resources.Database.TaskEntity> GetAllTasks()
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    return DbContext.Tasks.ToList();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }

        public static async Task DeleteGuildConfig(IGuild guild)
        {
            await DeleteGuildConfig(guild.Id);
        }

        public static async Task DeleteGuildConfig(ulong guildId)
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    if (DbContext.GuildConfig.Where(x => x.GuildId == guildId).Count() < 1)
                        return;
                    DbContext.GuildConfig.Remove(DbContext.GuildConfig.Where(x => x.GuildId == guildId).FirstOrDefault());
                    await DbContext.SaveChangesAsync();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }
        public static List<GuildConfigEntity> GetAllGuilds()
        {
            try
            { 
                using (var DbContext = new SqliteDbContext())
                {
                    return DbContext.GuildConfig.ToList();
                }
            }
            catch
            {
                //TODO: Log exception, more specified description
                throw;
            }
        }
    }
}
