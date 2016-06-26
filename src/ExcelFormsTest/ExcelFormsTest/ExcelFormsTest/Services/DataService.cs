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


        public static async Task<string> SaveReceiptImage(Stream image)
        {
            string id = string.Empty;

            if (IsLoggedIn)
            {
                id = await UploadFile(image, $"{Guid.NewGuid().ToString()}.jpeg", "image/jpeg");
            }

            return id;
        }

        public static async Task<string> UpdateReceiptImage(Stream image, string id)
        {
            if (!IsLoggedIn)
                return id;

            var newId = id;
            if (await RemoveReceiptImage(id))
            {
                newId = await SaveReceiptImage(image);
            }

            return newId;
        }

        public static async Task<bool> RemoveReceiptImage(string id)
        {
            var result = false;

            if(IsLoggedIn)
            {
                result = await DeleteFile(id);
            }
            return result;
        }

        private static async Task<bool> DeleteFile(string id)
        {
            if (!IsLoggedIn)
                return false;

            string fileLocation = $"{BaseGraphURL}/{GraphVersion}/{BaseItemPath}/{id}";
            var result = await client.DeleteAsync(fileLocation);
            return result.IsSuccessStatusCode;
        }

        public static async Task<string> GetReceiptImageAsBase64(string id)
        {
            var answer = string.Empty;
            if (!IsLoggedIn)
            {
                return answer;
            }

            var fileLocation = $"{BaseGraphURL}/{GraphVersion}/{BaseItemPath}/{id}";
            var result = await client.GetAsync(fileLocation);
            if (!result.IsSuccessStatusCode)
            {
                return answer;
            }

            var content = await result.Content.ReadAsStringAsync();
            var fileItem = JsonConvert.DeserializeObject<FileItem>(content);
            if (!string.IsNullOrEmpty(fileItem?.downloadUrl))
            {
                try
                {
                    var fileBytes = await client.GetByteArrayAsync(fileItem.downloadUrl);
                    answer = Convert.ToBase64String(fileBytes);
                }
                catch (Exception e)
                {

                }
            }
            return answer;
        }

        public static async Task<string> UploadFile(Stream stream, string fileName, string contentType)
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
                var answer = JsonConvert.DeserializeObject<FileItem>(json);
                var id = answer?.id ?? string.Empty;
                return id;
            }
            else
            {
                return string.Empty;
            }

        }

        public static async Task<bool> AddRows(List<ExpenseRow> rows)
        {
            if (!await EnsureConfig()) return false;

            var postString = $"{BaseGraphURL}/{GraphVersion}/{BaseItemPath}/{workbookID}/workbook/worksheets('{DataSheetName}')/Tables('{DataTableName}')/rows";

            var rowArray = new object[rows.Count];

            int thisRow = 0;
            HttpResponseMessage result = null;
            foreach (var row in rows)
            {
                rowArray[thisRow] = row.AsExcelRowObject();
                thisRow++;
                result = await client.PostAsync(postString, new StringContent(JsonConvert.SerializeObject(row.AsExcelRow()), Encoding.UTF8, "application/json"));

            }
            //object rowsObject = new
            //{
            //    values = rowArray
            //};
            //result = await client.PostAsync(postString, new StringContent(JsonConvert.SerializeObject(rowsObject), Encoding.UTF8, "application/json"));
            return result?.IsSuccessStatusCode ?? false;

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
                        // this Parse madness is because an exact number of dollars comes back as a long and as double? returns null!
                        Amount = double.Parse(item[2].ToString()),
                        Id = item[3] as string
                    });
                }
            }

            return rows;
        }

        internal static async Task AddSampleData()
        {
            var RowsToAdd = new List<ExpenseRow>();

            RowsToAdd.Add( new ExpenseRow()
            {
                Vendor = "Qantas Airways",
                Category = "Airfare",
                Amount = 174.87d,
                Id = string.Empty
            });

            RowsToAdd.Add( new ExpenseRow()
            {
                Vendor = "Qantas Airways",
                Category = "Airfare",
                Amount = 483.36d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "Singapore Airlines",
                Category = "Airfare",
                Amount = 1485.20d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "Virgin Australia",
                Category = "Airfare",
                Amount = 266.33d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "Telstra",
                Category = "Connectivity",
                Amount = 70.20d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "Vodafone",
                Category = "Connectivity",
                Amount = 60.00d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "T-Mobile",
                Category = "Connectivity",
                Amount = 25.47d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "Uber",
                Category = "Taxi",
                Amount = 18.27d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "Uber",
                Category = "Taxi",
                Amount = 12.56d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "Uber",
                Category = "Taxi",
                Amount = 42.90d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "Cafe 9",
                Category = "Meals",
                Amount = 12.50d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "McDonalds",
                Category = "Meals",
                Amount = 8.45d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "Thai the Knot",
                Category = "Meals",
                Amount = 36.21d,
                Id = string.Empty
            });

            RowsToAdd.Add(new ExpenseRow()
            {
                Vendor = "Cafe 9",
                Category = "Meals",
                Amount = 18.30d,
                Id = string.Empty
            });
            await AddRows(RowsToAdd);


        }

        public async static Task<string> GetChartImageAsBase64()
        {
            if (!await EnsureConfig()) return null;

            var queryString = $"{BaseGraphURL}/{GraphVersion}/{BaseItemPath}/{workbookID}/workbook/worksheets('{ChartSheetName}')/Charts('Chart 1')/Image";
            var json = await client.GetStringAsync(queryString);
            var jsonObj = JsonConvert.DeserializeObject<ChartGraphObject>(json);
            return jsonObj.value;
        }

        public static async Task<bool> AddRow(ExpenseRow row)
        {
            if (!await EnsureConfig()) return false;

            // adds a row to the Excel Datasource
            var postString = $"{BaseGraphURL}/{GraphVersion}/{BaseItemPath}/{workbookID}/workbook/worksheets('{DataSheetName}')/Tables('{DataTableName}')/rows";
            var xlRow = row.AsExcelRow();
            var result = await client.PostAsync(postString, new StringContent(JsonConvert.SerializeObject(xlRow), Encoding.UTF8, "application/json"));
            return result.IsSuccessStatusCode;
        }

        public static async Task<ExpenseRow> UpdateRow(int index, ExpenseRow row)
        {
            var address = $"{DataSheetName}!A{index + 2}:D{index + 2}";
            var updateString = $"{BaseGraphURL}/{GraphVersion}/{BaseItemPath}/{workbookID}/workbook/worksheets('{DataSheetName}')/range(address='{address}')";
            var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), updateString);
            patchRequest.Content = new StringContent(JsonConvert.SerializeObject(row.AsExcelRow()), Encoding.UTF8, "application/json");
            var patchResult = await client.SendAsync(patchRequest);

            if (patchResult.IsSuccessStatusCode)
            {
                var json = await patchResult.Content.ReadAsStringAsync();

                var answerObject = JsonConvert.DeserializeObject<UpdateRowRootObject>(json);
                var updatedRow = new ExpenseRow()
                {
                    Vendor = answerObject.values[0][0].ToString(),
                    Category = answerObject.values[0][1].ToString(),
                    Amount = double.Parse(answerObject.values[0][2].ToString()),
                    Id = answerObject.values[0][3].ToString()
                };

                return updatedRow;
            }
            else
            {
                return null;
            }
        }

        public static async Task<bool> DeleteRow(int index)
        {
            var address = $"{DataSheetName}!A{index + 2}:D{index + 2}";
            var deleteString = $"{BaseGraphURL}/{GraphVersion}/{BaseItemPath}/{workbookID}/workbook/worksheets('{DataSheetName}')/range(address='{address}')/delete";
            var moveUpInstructions = new { shift = "Up" };
            var result = await client.PostAsync(deleteString, new StringContent(JsonConvert.SerializeObject(moveUpInstructions)));
            return result.IsSuccessStatusCode;
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
                values = new object[] { this.AsExcelRowObject() }
            };
        }

        public object AsExcelRowObject()
        {
            return new object[] { Vendor, Category, Amount, Id };
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
    
    
    #region JsonConvert Classes
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

    public class FileItem
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


    public class UpdateRowRootObject
    {
        [JsonProperty("@odata.context")]
        public string context { get; set; }
        [JsonProperty("@odata.type")]
        public string type { get; set; }
        [JsonProperty("@odata.id")]
        public string id { get; set; }
        public string address { get; set; }
        public string addressLocal { get; set; }
        public int cellCount { get; set; }
        public int columnCount { get; set; }
        public bool columnHidden { get; set; }
        public int columnIndex { get; set; }
        public List<List<object>> formulas { get; set; }
        public List<List<object>> formulasLocal { get; set; }
        public List<List<object>> formulasR1C1 { get; set; }
        public bool hidden { get; set; }
        public List<List<string>> numberFormat { get; set; }
        public int rowCount { get; set; }
        public bool rowHidden { get; set; }
        public int rowIndex { get; set; }
        public List<List<string>> text { get; set; }
        public List<List<object>> values { get; set; }
        public List<List<string>> valueTypes { get; set; }
    }

    #endregion
}
