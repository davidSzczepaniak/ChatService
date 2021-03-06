﻿using System;
using System.Windows.Input;

namespace ChatService.ClientApp.ViewModels
{
    /// <summary>
    /// Implementation of ICommand interface for interaction with user interface
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        #endregion // ICommand Members
    }

}
