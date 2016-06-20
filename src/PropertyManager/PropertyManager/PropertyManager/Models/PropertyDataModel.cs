using Newtonsoft.Json.Linq;

namespace PropertyManager.Models
{
    public class PropertyDataModel : TableRowModel
    {
        public string Id
        {
            get { return this[0].Value<string>(); }
            set { this[0] = value; }
        }

        public string Description
        {
            get { return this[1].Value<string>(); }
            set { this[1] = value; }
        }

        public string Rooms
        {
            get { return GetString(2); }
            set { TrySetInt(2, value); }
        }

        public string LivingArea
        {
            get { return GetString(3); }
            set { TrySetInt(3, value); }
        }

        public string LotSize
        {
            get { return GetString(4); }
            set { TrySetInt(4, value); }
        }

        public string OperatingCosts
        {
            get { return GetString(5); }
            set { TrySetInt(5, value); }
        }

        public PropertyDataModel(TableRowModel row = null)
        {
            for (var i = 0; i < Constants.ExcelPropertyTableColumns; i++)
            {
                Add(row?[i]);
            }
        }

        private string GetString(int tokenIndex)
        {
            var token = this[tokenIndex];

            // Value is of type string when empty.
            return token.Type == JTokenType.String
                ? token.Value<string>()
                : (token.Value<int?>())?.ToString();
        }

        private void TrySetInt(int tokenIndex, string value)
        {
            // Remove value if null or whitespace.
            if (string.IsNullOrWhiteSpace(value))
            {
                this[tokenIndex] = "";
                return;
            }

            // Try to parse.
            int i;
            if (int.TryParse(value, out i))
            {
                this[tokenIndex] = i;
            }
        }
    }
}
