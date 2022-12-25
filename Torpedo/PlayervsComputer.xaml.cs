using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for PlayervsComputer.xaml
    /// </summary>
    public partial class PlayervsComputer : Page
    {
        public PlayervsComputer()
        {
            InitializeComponent();
        }
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Beginning());
        }
        private void OnStart(object sender, RoutedEventArgs e)
        {
            var newWindow = new MainWindow();
            newWindow.Show(); //Lehet nem sima Show() kell ez okozhat kesobb problemakat
            App.Current.MainWindow.Close();
        }
    }
}
