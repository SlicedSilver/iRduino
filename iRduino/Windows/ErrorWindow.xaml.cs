//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Windows
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow
    {
        public ErrorWindow(string errorText)
        {
            InitializeComponent();
            ErrorTextBox.Text = errorText;
        }
    }
}
