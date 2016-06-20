namespace PropertyManager
{
    public static class Constants
    {
        // Strings.
        public static string ExcelFileName => "Data.xlsx";
        public static string ExcelFileResourceName => 
            "PropertyManager.Resources." + ExcelFileName;
        public static string ExcelContentType => "application/xlsx";

        // File Extensions.
        public static string[] MediaFileExtensions => 
            new [] { ".png", ".jpg", ".jpeg" };
        public static string[] DocumentFileExtensions => 
            new [] { ".docx", ".xlsx", ".one", ".pptx" };
    }
}
