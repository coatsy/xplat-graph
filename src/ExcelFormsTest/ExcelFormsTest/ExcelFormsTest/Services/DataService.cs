using ExcelFormsTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.Services
{
    public static class DataService
    {
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
            if(prop != null)
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
            if(prop!=null)
            {
                propList.Remove(prop);
            }
        }


        #endregion
    }
}
