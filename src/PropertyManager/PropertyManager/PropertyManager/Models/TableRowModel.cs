using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PropertyManager.Models
{
    public class TableRowModel : List<JToken>
    {
        public TableRowModel()
        {
            
        }

        public TableRowModel(IEnumerable<JToken> collection)
            : base(collection)
        {
            
        }
    }
}
