using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PropertyManager.Models
{
    public class TableRowModel : List<JToken>
    {
        public static TableRowModel Create(string id, string description,
            int rooms, int livingArea, int lotSize, int operatingCosts)
        {
            return new TableRowModel
            {
                id,
                description,
                rooms,
                livingArea,
                lotSize,
                operatingCosts
            };
        }
    }
}
