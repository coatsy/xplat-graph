using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.Models
{
    public class PropertyInformationModel
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public int Bedrooms { get; set; }
        public int CarSpaces { get; set; }
        public int Bathrooms { get; set; }
        public Uri PictureLink { get; set; }
        public double Valuation { get; set; }
    }
}
