using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NaTruTraveller.ViewModels
{
    public class HomeVM
    {
        public NewRecordCommand NewRecordCommand { get; set; }

        public HomeVM()
        {
            NewRecordCommand = new NewRecordCommand(this);
        }

        public void NewRecordNavigation()
        {
            App.Current.MainPage.Navigation.PushAsync(new NewRecordPage());
        }
    }

    public class NewRecordCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private HomeVM vM;
        public NewRecordCommand(HomeVM vm)
        {
            vM = vm;
        }
      
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            vM.NewRecordNavigation();
        }
    }
}
