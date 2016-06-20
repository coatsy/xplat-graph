using System.Collections.Generic;

namespace PropertyManager.Models
{
    public class TableColumnModel
    {
        public string Id { get; set; }

        public int Index { get; set; }

        public string Name { get; set; }

        public List<List<object>> Values { get; set; }
    }
}
