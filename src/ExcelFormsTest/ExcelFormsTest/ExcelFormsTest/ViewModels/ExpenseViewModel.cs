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

        public ExpenseViewModel(Row row)
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
            set { if (category == value) return; category = value; NotifyPropertyChanged(); }
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

        public Image ReceiptImage
        {
            get { return GetReceiptImage(); }
        }

        private Image GetReceiptImage()
        {
            throw new NotImplementedException();
        }

        public Image CategoryImage
        {
            get { return GetCategoryImage(); }
        }

        private Image GetCategoryImage()
        {
            throw new NotImplementedException();
        }


    }
}
