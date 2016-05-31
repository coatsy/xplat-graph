using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.Services
{
    public static class LocationService
    {
        public static async Task<Location> GetLocation()
        {
            return await GetMockLocation();
        }

        #region Mock Data Methods
        private static async Task<Location> GetMockLocation()
        {
            // middle of Centennial Park
            return new Location()
            {
                Latitude = -33.9,
                Longitude = 151.23
            };
        }
        #endregion
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
