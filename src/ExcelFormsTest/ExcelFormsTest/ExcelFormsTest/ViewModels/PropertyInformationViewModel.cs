using ExcelFormsTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.ViewModels
{
    public class PropertyInformationViewModel : ViewModelBase
    {

        public PropertyInformationViewModel() { }

        public PropertyInformationViewModel(PropertyInformationModel pim)
        {
            id = pim.Id;
            latitude = pim.Latitude;
            longitude = pim.Longitude;
            address = pim.Address;
            bedrooms = pim.Bedrooms;
            carSpaces = pim.CarSpaces;
            bathrooms = pim.Bathrooms;
            pictureLink = pim.PictureLink;
            valuation = pim.Valuation;

            NotifyAllPropertiesChanged();

        }


        private int id;
        public int Id
        {
            get { return id; }
            set { if (id == value) return; id = value; NotifyPropertyChanged(); }
        }

        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set { if (latitude == value) return; latitude = value; NotifyPropertyChanged(); }
        }

        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set { if (longitude == value) return; longitude = value; NotifyPropertyChanged(); }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set { if (address == value) return; address = value; NotifyPropertyChanged(); }
        }

        private int bedrooms;
        public int Bedrooms
        {
            get { return bedrooms; }
            set { if (bedrooms == value) return; bedrooms = value; NotifyPropertyChanged(); }
        }

        private int carSpaces;
        public int CarSpaces
        {
            get { return carSpaces; }
            set { if (carSpaces == value) return; carSpaces = value; NotifyPropertyChanged(); }
        }

        private int bathrooms;
        public int Bathrooms
        {
            get { return bathrooms; }
            set { if (bathrooms == value) return; bathrooms = value; NotifyPropertyChanged(); }
        }

        private Uri pictureLink;
        public Uri PictureLink
        {
            get { return pictureLink; }
            set { if (pictureLink == value) return; pictureLink = value; NotifyPropertyChanged(); }
        }

        private double valuation;
        public double Valuation
        {
            get { return valuation; }
            set { if (valuation == value) return; valuation = value; NotifyPropertyChanged(); }
        }


    }
}
