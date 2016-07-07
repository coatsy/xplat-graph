﻿using System.IO;
using System.Threading.Tasks;
using PropertyManager.Models;

namespace PropertyManager.Services
{
    public interface IGraphService
    {
        Task<UserModel> GetUserAsync();

        Task<GroupModel[]> GetUserGroupsAsync();

        Task<DriveItemModel[]> GetUserDriveItemAsync();

        Task<DriveItemModel> AddUserDriveItemAsync(string name, Stream stream, string contentType);

        Task<GroupModel[]> GetGroupsAsync();

        Task<UserModel[]> GetGroupUsersAsync(GroupModel group);

        Task<DriveItemModel[]> GetGroupDriveItemsAsync(GroupModel group);

        Task<ConversationModel[]> GetGroupConversationsAsync(GroupModel group);

        Task<GroupModel> AddGroupAsync(GroupModel group);

        Task<DriveItemModel> AddGroupDriveItemAsync(GroupModel group, string name, Stream stream, string contentType);

        Task AddGroupUserAsync(GroupModel group, UserModel user);

        Task<TableModel<T>> GetTableAsync<T>(DriveItemModel driveItem, string tableName, GroupModel group = null) where T : TableRowModel, new();

        Task<TableColumnModel[]> GetTableColumnsAsync(DriveItemModel driveItem, string tableName, GroupModel group = null);

        Task<TableRowModel> AddTableRowAsync(DriveItemModel driveItem, string tableName, TableRowModel tableRow, GroupModel group = null);

        Task<TableRowsModel> UpdateTableRowsAsync(DriveItemModel driveItem, string sheetName, string address, TableRowModel[] tableRows, GroupModel group = null);
    }
}