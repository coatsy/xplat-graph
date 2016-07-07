namespace PropertyManager
{
    public static class Constants
    {
        public static string AppGroupDisplayName => "Property Managers";
        public static string AppGroupDescription => "Group for all of the users of the Property Manager app.";
        public static string AppGroupMail => "propertymanagerapp2";

        public static string DataFileName => "Data.xlsx";
        public static string DataFileResourceName => 
            "PropertyManager.Resources." + DataFileName;
        public static string DataFileDataSheet => "Data";
        public static string DataFilePropertyTable => "PropertyTable";
        public static string DataFilePropertyTableColumnStart => "A";
        public static string DataFilePropertyTableColumnEnd => "F";
        public static int DataFilePropertyTableColumns => 6;

        public static string ExcelContentType => "application/xlsx";
        public static string JsonContentType => "application/json";

        public static string[] MediaFileExtensions => 
            new [] { ".png", ".jpg", ".jpeg" };
        public static string[] DocumentFileExtensions => 
            new [] { ".docx", ".xlsx", ".one", ".pptx" };
    }
}
