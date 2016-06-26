using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelFormsTest.Services;
using Xamarin.Forms;
using System.IO;
using Plugin.Media;

namespace ExcelFormsTest.ViewModels
{
    public class ExpenseViewModel : ViewModelBase
    {

        public List<String> ExpenseCategories
        {
            get { return DataService.ExpenseCategories; }
        }

        public ExpenseViewModel(ExpenseRow row)
        {
            this.Vendor = row.Vendor;
            this.Category = row.Category;
            this.Amount = row.Amount;
            this.ReceiptId = row.Id;

            InitialiseCamera();

        }

        private async void InitialiseCamera()
        {
            CameraInitialised = await CrossMedia.Current.Initialize();
        }

        private bool cameraInitialised;
        public bool CameraInitialised
        {
            get { return cameraInitialised; }
            set { if (cameraInitialised == value) return; cameraInitialised = value; NotifyPropertyChanged(); }
        }


        private string vendor;
        public string Vendor
        {
            get { return vendor; }
            set { if (vendor == value) return; vendor = value; NotifyPropertyChanged(); }
        }

        private string category;
        public string Category
        {
            get { return category; }
            set { if (category == value) return; category = value; NotifyPropertyChanged(); NotifyPropertyChanged("CategoryImage"); }
        }

        private double amount;
        public double Amount
        {
            get { return amount; }
            set { if (amount == value) return; amount = value; NotifyPropertyChanged(); }
        }


        private string receiptId;

        public string ReceiptId
        {
            get { return receiptId; }
            set
            {
                if (receiptId == value) return;
                receiptId = value;
                NotifyPropertyChanged();
                GetReceiptImageBase64(receiptId);
                NotifyPropertyChanged("ReceiptPresentImage");
            }
        }

        private async void GetReceiptImageBase64(string id)
        {
            ReceiptImageBase64 = await DataService.GetReceiptImageAsBase64(id);
        }

        private string receiptImageBase64;
        public string ReceiptImageBase64
        {
            get { return receiptImageBase64; }
            set { if (receiptImageBase64 == value) return; receiptImageBase64 = value; NotifyPropertyChanged(); NotifyPropertyChanged("ReceiptImage"); }
        }


        public ImageSource ReceiptImage
        {
            get
            {
                return string.IsNullOrEmpty(ReceiptImageBase64) ?
                    null :
                    ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(ReceiptImageBase64)));
            }
        }

        public ImageSource CategoryImage
        {
            get { return GetResourceImage(Category); }
        }

        public ImageSource ReceiptPresentImage
        {
            get
            {
                return string.IsNullOrEmpty(ReceiptId) ?
                    GetResourceImage("NoReceipt") :
                    GetResourceImage("Receipt");
            }
        }
        
        private const string resourceTemplate = "ExcelFormsTest.Resources.Images.{0}.png";
        private ImageSource GetResourceImage(string imageName)
        {
            ImageSource src = null;
            var resource = string.Format(resourceTemplate, imageName);
            src = ImageSource.FromResource(resource);
            if(src == null)
            {
                resource = string.Format(resourceTemplate, "Default");
                src = ImageSource.FromResource(resource);
            }
            return src;
        }

        private CommandBase getReceiptImageFromCameraCommand;

        public CommandBase GetReceiptImageFromCameraCommand
        {
            get
            {
                getReceiptImageFromCameraCommand = getReceiptImageFromCameraCommand ?? new CommandBase(DoGetReceiptImageFromCameraCommand);
                return getReceiptImageFromCameraCommand;
            }
        }

        private async void DoGetReceiptImageFromCameraCommand()
        {
            if(!CameraInitialised)
            {
                Message = "Waiting for camera to initialise, please tray again shortly";
            }
            else if(!CrossMedia.Current.IsCameraAvailable)
            {
                Message = "Camera unavailable on this hardware";
            }
            else
            {
                var imageFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                {
                    Name = $"{Guid.NewGuid().ToString()}.jpeg",
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Directory = "Temp",
                    SaveToAlbum = true
                }
                );

                Message = "Saving ...";

                if (imageFile == null)
                {
                    Message = "Cancelled";
                }
                else
                {
                    ReceiptId = await DataService.UploadFile(imageFile.GetStream(), Path.GetFileName(imageFile.Path), "image/jpeg");
                    Message = "Done";
                }
            }
        }

        private CommandBase getReceiptImageFromLibraryCommand;

        public CommandBase GetReceiptImageFromLibraryCommand
        {
            get
            {
                getReceiptImageFromLibraryCommand = getReceiptImageFromLibraryCommand ?? new CommandBase(DoGetReceiptImageFromLibraryCommand);
                return getReceiptImageFromLibraryCommand;
            }
        }

        private async void DoGetReceiptImageFromLibraryCommand()
        {
            if (!CameraInitialised)
            {
                Message = "Waiting for image stack to initialise, please tray again shortly";
            }
            else if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Message = "Photo selection unavailable on this hardware";
            }
            else
            {
                var imageFile = await CrossMedia.Current.PickPhotoAsync();
                if (imageFile == null)
                {
                    Message = "Cancelled";
                }
                else
                {
                    ReceiptId = await DataService.UploadFile(imageFile.GetStream(), $"{Guid.NewGuid().ToString()}.jpeg", "image/jpeg");
                    Message = "Done";
                }
            }
        }
    }
}
