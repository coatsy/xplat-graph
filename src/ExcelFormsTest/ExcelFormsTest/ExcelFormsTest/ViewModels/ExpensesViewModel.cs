using ExcelFormsTest.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExcelFormsTest.ViewModels
{
    public class ExpensesViewModel : ViewModelBase
    {
        public ExpensesViewModel()
        {
            Title = "Expense List";
            DoGetExpensesCommand();
        }

        public bool IsExpenseSelected
        {
            get { return SelectedExpense != null; }
        }

        public int SelectedExpenseIndex
        {
            get { return SelectedExpense == null ? -1 : Expenses.IndexOf(SelectedExpense); }
        }

        private ExpenseViewModel selectedExpense;
        public ExpenseViewModel SelectedExpense
        {
            get { return selectedExpense; }
            set
            {
                if (selectedExpense == value) return;
                selectedExpense = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SelectedExpenseIndex");
                NotifyPropertyChanged("IsExpenseSelected");
            }
        }



        private ObservableCollection<ExpenseViewModel> expenses;
        public ObservableCollection<ExpenseViewModel> Expenses
        {
            get { expenses = expenses ?? new ObservableCollection<ExpenseViewModel>(); return expenses; }
        }


        private CommandBase getExpensesCommand;

        public CommandBase GetExpensesCommand
        {
            get
            {
                getExpensesCommand = getExpensesCommand ?? new CommandBase(DoGetExpensesCommand);
                return getExpensesCommand;
            }
        }

        private async void DoGetExpensesCommand()
        {
            ClearMessage();
            var isRefreshing = IsRefreshing;
            IsRefreshing = true;
            var rows = await DataService.GetRows();
            Expenses.Clear();
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    Expenses.Add(new ExpenseViewModel(row));
                }
            }

            IsRefreshing = isRefreshing;
        }



        private CommandBase deleteExpenseCommand;

        public CommandBase DeleteExpenseCommand
        {
            get
            {
                deleteExpenseCommand = deleteExpenseCommand ?? new CommandBase(DoDeleteExpenseCommand);
                return deleteExpenseCommand;
            }
        }

        private async void DoDeleteExpenseCommand()
        {
            ClearMessage();
            if (!IsExpenseSelected)
            {
                ShowMessage("No Expense Selected");
                return;
            }
            var success = await DataService.DeleteRow(SelectedExpenseIndex);
            if (!success)
            {
                ShowMessage("Error deleting row");
            }
            else
            {
                Expenses.RemoveAt(SelectedExpenseIndex);
            }
        }

        private CommandBase addExpenseCommand;

        public CommandBase AddExpenseCommand
        {
            get
            {
                addExpenseCommand = addExpenseCommand ?? new CommandBase(DoAddExpenseCommand);
                return addExpenseCommand;
            }
        }

        private async void DoAddExpenseCommand()
        {
            ClearMessage();
            ExpenseRow row = new ExpenseRow()
            {
                Vendor = "Vodafone",
                Category = "Connectivity",
                Amount = 128d,
                Id = string.Empty
            };

            var success = await DataService.AddRow(row);

            if (success)
            {
                Expenses.Add(new ExpenseViewModel(row));
            }
            else
            {
                ShowMessage("Error adding expense");
            }
        }

        private CommandBase updateExpenseCommand;

        public CommandBase UpdateExpenseCommand
        {
            get
            {
                updateExpenseCommand = updateExpenseCommand ?? new CommandBase(DoUpdateExpenseCommand);
                return updateExpenseCommand;
            }
        }

        private async void DoUpdateExpenseCommand()
        {

            ClearMessage();
            if (!IsExpenseSelected)
            {
                ShowMessage("No Expense Selected");
                return;
            }

            var newExpense = new ExpenseRow()
            {
                Vendor = SelectedExpense.Vendor,
                Category = SelectedExpense.Category,
                Amount = SelectedExpense.Amount,
                Id = SelectedExpense.ReceiptId
            };

            newExpense.Amount += 10d;

            var updatedRow = await DataService.UpdateRow(SelectedExpenseIndex, newExpense);

            if (updatedRow == null)
            {
                ShowMessage("Error Updating Row");
            }
            else
            {
                SelectedExpense.Vendor = updatedRow.Vendor;
                SelectedExpense.Category = updatedRow.Category;
                SelectedExpense.Amount = updatedRow.Amount;
                SelectedExpense.ReceiptId = updatedRow.Id;
            }
        }

        private CommandBase addSampleDataCommand;

        public CommandBase AddSampleDataCommand
        {
            get
            {
                addSampleDataCommand = addSampleDataCommand ?? new CommandBase(DoAddSampleDataCommand);
                return addSampleDataCommand;
            }
        }

        private async void DoAddSampleDataCommand()
        {
            var refreshing = IsRefreshing;
            IsRefreshing = true;
            await DataService.AddSampleData();
            DoGetExpensesCommand();
            IsRefreshing = refreshing;
        }
    }
}
