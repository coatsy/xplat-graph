namespace PropertyManager.Models
{
    public class DataFileModel
    {
        public DriveItemModel DriveItem { get; set; }
        
        public TableModel<PropertyTableRowModel> PropertyTable { get; set; } 
    }
}
