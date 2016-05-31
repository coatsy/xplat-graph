using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExcelFormsTest.ViewModels
{
    public class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action action;

        public CommandBase(Action _action)
        {
            action = _action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action.Invoke();
        }
    }
}
