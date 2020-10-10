using System;
using System.Windows.Input;

namespace MvvmInWPF.Command
{
    public class RelayCommand : ICommand
    {
        private bool withParam = false;
        private Action<object> execute;
        private Func<object, bool> canExecute;
        private Action parameterlessExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.withParam = true;
            this.execute = execute;
            this.canExecute = canExecute;
        } 
        
        public RelayCommand(Action execute, Func<object, bool> canExecute = null)
        {
            withParam = false;
            this.parameterlessExecute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (withParam)
            {
                this.execute?.Invoke(parameter);
            }
            else
            {
                this.parameterlessExecute?.Invoke();
            }
        }
    }
}