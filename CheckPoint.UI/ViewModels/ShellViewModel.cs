using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPoint.UI.ViewModels
{
    internal class ShellViewModel : ViewModelBase
    {
        public ShellViewModel()
        {
            // Build the menu.
            this.Menu.Add(new MenuItem() { Icon = new PackIconFontAwesome()
                { Kind = PackIconFontAwesomeKind.HomeSolid }, Text = "Home",
                NavigationDestination = new Uri("Pages/HomePage.xaml", 
                UriKind.RelativeOrAbsolute) });

            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome()
                { Kind = PackIconFontAwesomeKind.FolderOpenSolid },
                Text = "Project Library",
                NavigationDestination = new Uri("Pages/LibraryPage.xaml",
                UriKind.RelativeOrAbsolute)
            });
            
            this.OptionsMenu.Add(new MenuItem() { Icon = new PackIconFontAwesome()
            { Kind = PackIconFontAwesomeKind.CogSolid }, Text = "Settings",
                NavigationDestination = new Uri("Pages/SettingsPage.xaml", 
                UriKind.RelativeOrAbsolute) });

            this.OptionsMenu.Add(new MenuItem() { Icon = new PackIconFontAwesome()
            { Kind = PackIconFontAwesomeKind.InfoCircleSolid }, Text = "About",
                NavigationDestination = new Uri("Pages/AboutPage.xaml", 
                UriKind.RelativeOrAbsolute) });
        }

        public object GetItem(object uri)
        {
            return null == uri ? null : this.Menu.FirstOrDefault(m => m.NavigationDestination.Equals(uri));
        }

        public object GetOptionsItem(object uri)
        {
            return null == uri ? null : this.OptionsMenu.FirstOrDefault(m => m.NavigationDestination.Equals(uri));
        }
    }
}
