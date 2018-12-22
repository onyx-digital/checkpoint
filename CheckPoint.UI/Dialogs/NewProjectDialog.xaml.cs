using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CheckPoint.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for ProjectCommitDialog.xaml
    /// </summary>
    public partial class NewProjectDialog : Window
    {
        public NewProjectDialog()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txbProjectName.SelectAll();
            txbProjectName.Focus();
        }

        public string ProjectName
        {
            get { return txbProjectName.Text; }
        }

        public string LocalPath
        {
            get { return txbLocalPath.Text; }
        }
               
        public string RepoBranch
        {
            get { return txbRepoBranch.Text; }
        }
    }
}
