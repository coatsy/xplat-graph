using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.Models
{
    public class FileValue
    {
        [JsonProperty("@odata.type")]
        public string tag { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    public class FileList
    {
        [JsonProperty("@odata.context")]
        public string context { get; set; }
        [JsonProperty("value")]
        public List<FileValue> fileValues { get; set; }
    }

}
