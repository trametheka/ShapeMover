using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModels {
    internal class ViewModelCommand : ICommand {
        public event EventHandler? CanExecuteChanged;

        readonly Action<object?> action;
        readonly Predicate<object?> actionAvailable;
        internal ViewModelCommand(Action<object?> action) : this(action, (x => true)) { }
        internal ViewModelCommand(Action<object?> action, Predicate<object?> actionAvailable) {
            this.action = action;
            this.actionAvailable = actionAvailable;
        }

        public bool CanExecute(object? parameter) {
            return actionAvailable(parameter);
        }

        public void Execute(object? parameter) {
            action(parameter);
        }

        public void RaiseCanExecuteChanged() {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
