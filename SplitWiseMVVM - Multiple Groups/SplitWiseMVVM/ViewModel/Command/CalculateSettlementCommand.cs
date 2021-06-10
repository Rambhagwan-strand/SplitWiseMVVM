using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SplitWiseMVVM.ViewModel.Command
{
    class CalculateSettlementCommand : ICommand
    {
        public SplitWiseVM VM { get; set; }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public CalculateSettlementCommand(SplitWiseVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.PrintSettledShareForAllGroup(VM.GroupOfUsers, VM.ResultOfSettlement);
            
        }
    }
}
