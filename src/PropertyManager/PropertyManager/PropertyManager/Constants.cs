namespace PropertyManager
{
    public static class Constants
    {
        // Excel File.
        public static string ExcelFileName => "Data.xlsx";
        public static string ExcelFileResourceName => 
            "PropertyManager.Resources." + ExcelFileName;
        public static string ExcelDataSheet => "Data";
        public static string ExcelPropertyTable => "PropertyTable";
        public static string ExcelPropertyTableColumnStart => "A";
        public static string ExcelPropertyTableColumnEnd => "F";

        // Content Types.
        public static string ExcelContentType => "application/xlsx";
        public static string JsonContentType => "application/json";

        // File Extensions.
        public static string[] MediaFileExtensions => 
            new [] { ".png", ".jpg", ".jpeg" };
        public static string[] DocumentFileExtensions => 
            new [] { ".docx", ".xlsx", ".one", ".pptx" };
    }
}
