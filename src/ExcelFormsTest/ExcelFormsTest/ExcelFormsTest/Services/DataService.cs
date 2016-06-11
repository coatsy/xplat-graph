using ExcelFormsTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExcelFormsTest.Services
{
    public static class DataService
    {


        public static List<string> ExpenseCategories = new List<string>()
        {
            "Taxi",
            "Airfare",
            "Lodging",
            "Meals",
            "Supplies",
            "Gadgets",
            "Connectivity"
        };

        private static string token;

        public static string Token
        {
            get { return token; }
            set
            {
                token = value;
                if (string.IsNullOrEmpty(value))
                {
                    client.DefaultRequestHeaders.Authorization = null;
                }
                else
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Token);
                }
            }
        }

        public static bool IsLoggedIn
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Token);
            }
        }


        public static string BaseGraphURL { get; set; } = "https://graph.microsoft.com";
        public static string GraphVersion { get; set; } = "beta";
        public static string FilteredFileQuery { get; set; } = "me/drive/root/search(q='{0}')?$select=name,id";
        public static string AppRootPath { get; set; } = "me/drive/special/approot:";
        public static string BaseItemPath { get; set; } = "me/drive/items";
        public static string ExpensesSpreadsheetName { get; set; } = "Expenses.xlsx";
        public static string DataSheetName { get; set; } = "Data";
        public static string DataTableName { get; set; } = "Table1";
        public static string ChartSheetName { get; set; } = "Analysis";
        private static HttpClient client = new HttpClient();
        private static string workbookID = string.Empty;

        // ensures the "Expenses.xslx" file exists in the approot
        private static async Task<bool> EnsureConfig()
        {
            // Are we logged in - if not just return false
            if (!IsLoggedIn)
            {
                workbookID = string.Empty;
                return false;
            }

            // does the file already exist? if so, return true
            var check = await client.GetAsync($"{BaseGraphURL}/{GraphVersion}/{AppRootPath}/{ExpensesSpreadsheetName}?$select=name,id");
            if (check.IsSuccessStatusCode)
            {
                var json = await check.Content.ReadAsStringAsync();
                var answer = JsonConvert.DeserializeObject<FileListRootObject>(json);
                workbookID = answer?.id ?? string.Empty;
                return !string.IsNullOrWhiteSpace(workbookID);
            }

            // we're logged in but the file doesn't exist. Create it.
            // read the binary stream from the embedded resource
            var assembly = typeof(DataService).GetTypeInfo().Assembly;
            const string RESOURCE_NAME = "ExcelFormsTest.Resources.Expenses.xlsx";

            using (Stream stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            {
                workbookID = await UploadFile(stream, ExpensesSpreadsheetName, "application/xlsx");
            }

            return !string.IsNullOrWhiteSpace(workbookID);

        }

        private static async Task<string> UploadFile(Stream stream, string fileName, string contentType)
        {
            if (!IsLoggedIn)
            {
                return string.Empty;
            }

            StreamContent fileStream = new StreamContent(stream);
            fileStream.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            string fileLocation = $"{BaseGraphURL}/{GraphVersion}/{AppRootPath}/{fileName}:/content";
            var result = await client.PutAsync(fileLocation, fileStream);
            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                var answer = JsonConvert.DeserializeObject<FileCreationRootObject>(json);
                var id = answer?.id ?? string.Empty;
                return id;
            }
            else
            {
                return string.Empty;
            }

        }

        public static async Task<List<ExpenseRow>> GetRows()
        {
            List<ExpenseRow> rows = new List<ExpenseRow>();
            if (!await EnsureConfig())
            {
                return rows;
            }

            string queryString = $"{BaseGraphURL}/{GraphVersion}/{BaseItemPath}/{workbookID}/workbook/worksheets('{DataSheetName}')/tables('{DataTableName}')/rows";

            var rowList = await client.GetStringAsync(queryString);

            var rowsObject = JsonConvert.DeserializeObject<ExpenseRowsRootObject>(rowList);

            foreach (var row in rowsObject.value)
            {
                foreach (var item in row.values)
                {
                    rows.Add(new ExpenseRow()
                    {
                        Vendor = item[0] as string,
                        Category = item[1] as string,
                        Amount = item[2] as double? ?? 0.00,
                        Id = item[3] as string
                    });
                }
            }

            return rows;
        }

        public async static Task<string> GetChartImageAsBase64()
        {
            if (!await EnsureConfig()) return null;

            var patchString = $"{BaseGraphURL}/{GraphVersion}/{BaseItemPath}/{workbookID}/workbook/worksheets('{ChartSheetName}')/Charts('Chart 1')";

            var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), patchString);
            patchRequest.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            var patchResult = await client.SendAsync(patchRequest);



            var queryString = $"{BaseGraphURL}/{GraphVersion}/{BaseItemPath}/{workbookID}/workbook/worksheets('{ChartSheetName}')/Charts('Chart 1')/Image";
            var json = await client.GetStringAsync(queryString);
            var jsonObj = JsonConvert.DeserializeObject<ChartGraphObject>(json);
            return jsonObj.value;
        }

        public static async Task<List<PropertyInformationModel>> GetProperties()
        {
            return await GetMockPropertyData();
        }

        public static async Task<PropertyInformationModel> AddProperty(PropertyInformationModel pim)
        {
            return await AddPropertyToMockData(pim);
        }

        public static async Task<PropertyInformationModel> UpdateProperty(PropertyInformationModel pim)
        {
            return await UpdateMockProperty(pim);
        }

        public static async Task DeleteProperty(PropertyInformationModel pim)
        {
            await DeleteMockProperty(pim);
        }

        public static async Task<FileList> GetExcelFiles()
        {
            FileList result = new FileList();

            string rawResult = await GetFileList("xlsx");

            result = JsonConvert.DeserializeObject<FileList>(rawResult);

            return result;
        }

        private static async Task<string> GetFileList(string filter)
        {
            string answer = string.Empty;
            string queryString = string.Format($"{BaseGraphURL}/{GraphVersion}/{FilteredFileQuery}", filter);

            answer = await executeQuery(queryString);

            return answer;
        }

        private static async Task<string> executeQuery(string queryString)
        {
            string answer = string.Empty;

            if (string.IsNullOrEmpty(Token) || client.DefaultRequestHeaders.Authorization == null)
            {
                throw new NotAuthorisedException("Token must be set");
            }

            answer = await client.GetStringAsync(queryString);

            return answer;
        }


        #region Mock Data

        private static List<PropertyInformationModel> propList =
            new List<PropertyInformationModel>()
            {
                new PropertyInformationModel()
                {
                    Id = 1,
                    Latitude = -33.796689,
                    Longitude = 151.138338,
                    Address = "1 Epping Rd, NORTH RYDE NSW 2113",
                    Bedrooms = 4,
                    CarSpaces = 2,
                    Bathrooms = 3,
                    PictureLink = null,
                    Valuation = 1235000
                },
                new PropertyInformationModel()
                {
                    Id = 2,
                    Latitude = -33.796915,
                    Longitude = 151.139562,
                    Address = "16 Julius Ave NORTH RYDE NSW 2113",
                    Bedrooms = 3,
                    CarSpaces = 2,
                    Bathrooms = 2,
                    PictureLink = null,
                    Valuation = 1050000
                },
                new PropertyInformationModel()
                {
                    Id = 3,
                    Latitude = -33.795114,
                    Longitude = 151.139927,
                    Address = "39 Delhi Rd NORTH RYDE NSW 2113",
                    Bedrooms = 5,
                    CarSpaces = 4,
                    Bathrooms = 4,
                    PictureLink = null,
                    Valuation = 1830000
                },
                new PropertyInformationModel()
                {
                    Id = 4,
                    Latitude = -33.796569,
                    Longitude = 151.142659,
                    Address = "2 Newbigin Cl NORTH RYDE NSW 2113",
                    Bedrooms = 2,
                    CarSpaces = 1,
                    Bathrooms = 1,
                    PictureLink = null,
                    Valuation = 794000
                },
            };



        private static async Task<PropertyInformationModel> AddPropertyToMockData(PropertyInformationModel pim)
        {
            pim.Id = propList.Max(p => p.Id) + 1;
            propList.Add(pim);
            return pim;
        }


        private static async Task<List<PropertyInformationModel>> GetMockPropertyData()
        {
            return propList;
        }

        private static async Task<PropertyInformationModel> UpdateMockProperty(PropertyInformationModel pim)
        {
            var prop = propList.Where(p => p.Id == pim.Id).FirstOrDefault();
            if (prop != null)
            {
                prop.Latitude = pim.Latitude;
                prop.Longitude = pim.Longitude;
                prop.Address = pim.Address;
                prop.Bedrooms = pim.Bedrooms;
                prop.CarSpaces = pim.CarSpaces;
                prop.Bathrooms = pim.Bathrooms;
                prop.PictureLink = pim.PictureLink;
                prop.Valuation = pim.Valuation;
            }

            return prop;
        }

        private static async Task DeleteMockProperty(PropertyInformationModel pim)
        {
            var prop = propList.Where(p => p.Id == pim.Id).FirstOrDefault();
            if (prop != null)
            {
                propList.Remove(prop);
            }
        }


        #endregion
    }

    public class ExpenseRow
    {
        public string Vendor { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public string Id { get; set; }

        public object AsExcelRow()
        {
            return new
            {
                values = new object[] { Vendor, Category, Amount, Id }
            };
        }
    }

    public class NotAuthorisedException : Exception
    {
        private string message;

        public NotAuthorisedException()
        {
        }

        public NotAuthorisedException(string message) : base(message)
        {
        }

        public NotAuthorisedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    // used for deserializing json from the graph
    public class ExpenseRowsValue
    {
        [JsonProperty("@odata.id")]
        public string id { get; set; }
        public int index { get; set; }
        public List<List<object>> values { get; set; }
    }

    public class ExpenseRowsRootObject
    {
        [JsonProperty("@odata.context")]
        public string context { get; set; }
        public List<ExpenseRowsValue> value { get; set; }
    }


    public class ChartGraphObject
    {
        [JsonProperty("@odata.context")]
        public string context { get; set; }
        public string value { get; set; }
    }

    public class FileListRootObject
    {
        [JsonProperty("@odata.context")]
        public string context { get; set; }
        [JsonProperty("@odata.etag")]
        public string etag { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }


    public class GraphApplication
    {
        public string displayName { get; set; }
        public string id { get; set; }
    }

    public class GraphUser
    {
        public string displayName { get; set; }
        public string id { get; set; }
    }

    public class CreatedBy
    {
        public GraphApplication application { get; set; }
        public GraphUser user { get; set; }
    }


    public class LastModifiedBy
    {
        public GraphApplication application { get; set; }
        public GraphUser user { get; set; }
    }

    public class ParentReference
    {
        public string driveId { get; set; }
        public string id { get; set; }
        public string path { get; set; }
    }

    public class Hashes
    {
        public string crc32Hash { get; set; }
        public string sha1Hash { get; set; }
    }

    public class GraphFile
    {
        public Hashes hashes { get; set; }
        public string mimeType { get; set; }
    }

    public class GraphFileSystemInfo
    {
        public string createdDateTime { get; set; }
        public string lastModifiedDateTime { get; set; }
    }

    public class FileCreationRootObject
    {
        [JsonProperty("@odata.context")]
        public string context { get; set; }
        [JsonProperty("@microsoft.graph.downloadUrl")]
        public string downloadUrl { get; set; }
        public CreatedBy createdBy { get; set; }
        public string createdDateTime { get; set; }
        public string cTag { get; set; }
        public string eTag { get; set; }
        public string id { get; set; }
        public LastModifiedBy lastModifiedBy { get; set; }
        public string lastModifiedDateTime { get; set; }
        public string name { get; set; }
        public ParentReference parentReference { get; set; }
        public int size { get; set; }
        public string webUrl { get; set; }
        public GraphFile file { get; set; }
        public GraphFileSystemInfo fileSystemInfo { get; set; }
    }

}
