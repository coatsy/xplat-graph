using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelFormsTest.Services;
using Xamarin.Forms;

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
            set { if (receiptId == value) return; receiptId = value; NotifyPropertyChanged(); NotifyPropertyChanged("ReceiptImage"); }
        }

        public ImageSource ReceiptImage
        {
            get { return GetReceiptImage(); }
        }

        private ImageSource GetReceiptImage()
        {
            //TODO Implement the Base64 Stuff from chart
            return null;
        }

        public Image CategoryImage
        {
            get { return GetCategoryImage(); }
        }

        private Image GetCategoryImage()
        {
            throw new NotImplementedException();
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

        private void DoGetReceiptImageFromCameraCommand()
        {

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

        private void DoGetReceiptImageFromLibraryCommand()
        {

        }
    }
}
