namespace PropertyManager
{
    public static class Constants
    {
        // Strings.
        public static string ExcelFileName => "Data.xlsx";
        public static string ExcelFileResourceName => 
            "PropertyManager.Resources." + ExcelFileName;
        public static string ExcelContentType => "application/xlsx";
        public static string ExcelPropertyTable => "PropertyTable";
        public static string ExcelIDColumn => "ID";
        public static string ExcelDescriptionColumn => "Description";
        public static string ExcelRoomsColumn => "Rooms";
        public static string ExcelLivingAreaColumn => "LivingArea";
        public static string ExcelLotSizeColumn => "LotSize";
        public static string ExcelLotOperatingCostsColumn => "OperatingCosts";

        // File Extensions.
        public static string[] MediaFileExtensions => 
            new [] { ".png", ".jpg", ".jpeg" };
        public static string[] DocumentFileExtensions => 
            new [] { ".docx", ".xlsx", ".one", ".pptx" };
    }
}
