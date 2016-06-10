using ExcelFormsTest.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFormsTest.ViewModels
{
    public class ExpensesViewModel : ViewModelBase
    {
        public ExpensesViewModel()
        {
            Title = "Expense List";
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
            var rows = await DataService.GetRows();
            Expenses.Clear();
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    Expenses.Add(new ExpenseViewModel(row));
                }
            }
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

        private void DoDeleteExpenseCommand()
        {

        }
    }
}
