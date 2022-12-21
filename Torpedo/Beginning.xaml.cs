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
    /// Interaction logic for Beginning.xaml
    /// </summary>
    public partial class Beginning : Page
    {
        public Beginning()
        {
            InitializeComponent();
        }

        private void OnPlayer1vsPlayer2(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Player1vsPlayer2());
        }

        private void OnPlayervsComputer(object sender, RoutedEventArgs e)
        {
            
        }

        private void OnScores(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Scores());
        }
    }
}
