﻿using System.IO;
using System.Threading.Tasks;
using PropertyManager.Models;

namespace PropertyManager.Services
{
    public interface IGraphService
    {
        Task<GroupModel[]> GetGroupsAsync();

        Task<GroupModel[]> GetUserGroupsAsync();

        Task<DriveItemModel[]> GetGroupDriveItemsAsync(GroupModel group);

        Task<ConversationModel[]> GetGroupConversationsAsync(GroupModel group);

        Task<DriveItemModel> AddGroupDriveItemAsync(GroupModel group, string name, Stream stream, 
            string contentType);

        Task<DriveItemModel[]> GetDriveItemsAsync();

        Task<DriveItemModel> CreateDriveItemAsync(string name, Stream stream, string contentType);

        Task<TableColumnModel[]> GetTableColumnsAsync(DriveItemModel driveItem, string tableName);

        Task<TableRowModel> AddTableRowAsync(DriveItemModel driveItem, string tableName,
            TableRowModel tableRow);

        Task<TableRowsModel> UpdateTableRowAsync(DriveItemModel driveItem, string sheetName,
            string address, TableRowModel tableRow);
    }
}
