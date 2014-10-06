using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Input;

namespace iRduino.Commands
{

    public class ShowMainWindow : CommandBase<ShowMainWindow>
    {
        public override void Execute(object parameter)
        {
            GetTaskbarWindow(parameter).Show();
            GetTaskbarWindow(parameter).WindowState = WindowState.Normal;
            CommandManager.InvalidateRequerySuggested();
        }


        public override bool CanExecute(object parameter)
        {

            return true;
            //Window win = GetTaskbarWindow(parameter);
            //return win != null && !win.IsVisible;

        }
    }

}
