using CheckPoint.Core.Models;
using CheckPoint.UI.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CheckPoint.Core.Models.GitClient;

namespace CheckPoint.UI.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            UpdatePopularProjects();
            UpdateRecentProjects();
        }

        private void UpdatePopularProjects()
        {
            List<GitProject> projects = App.ProjectControl.ReadProjects();
            dgPopularProjects.ItemsSource = projects.OrderByDescending(p => p.TimesOpened).Take(3);
        }

        private void UpdateRecentProjects()
        {
            List<GitProject> projects = App.ProjectControl.ReadProjects();
            dgRecentProjects.ItemsSource = projects.OrderByDescending(p => p.LastOpened).Take(3);
        }
    }
}
